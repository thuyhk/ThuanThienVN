using HGT.Admin.Models;
using HGT.Core;
using HGT.Core.Enums;
using HGT.Core.Providers;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HGT.Admin.Extensions;
using System.IO;
using Repository.Pattern.Infrastructure;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using System.ComponentModel;
using HGT.Entities.Models;
using HGT.Admin.Filters;
using System.Linq.Expressions;
using AutoMapper;
using HGT.Core.Extensions;
using Newtonsoft.Json;
using HGT.Core.Utility;
using System.Drawing;
using HGT.Core.Models;
using System.Transactions;

namespace HGT.Admin.Controllers
{
    public class GalleryController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IGalleryService _galleryService;
        private readonly IGalleryDetailService _galleryDetailService;
        #endregion

        #region Contructors

        public GalleryController(IUnitOfWork unitOfWork, ICategoryService categoryService,
            IProductService productService, IGalleryService galleryService, IGalleryDetailService galleryDetailService)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._productService = productService;
            this._galleryService = galleryService;
            this._galleryDetailService = galleryDetailService;
        }

        #endregion

        #region Methods

        #region #index
        public ActionResult Index(int galleryId = 0, string redirectUrl = "")
        {
            if (galleryId <= 0)
                return RedirectToAction("Index", "Dashboard");
            ViewBag.GalleryId = galleryId;
            ViewBag.RedirectUrl = redirectUrl;
            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request, string galleryId)
        {
            int total = 0;
            int galId = Convert.ToInt32(galleryId);

            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("Id", ListSortDirection.Descending);

            sortDescriptor.Member = sortDescriptor.Member ?? "FileName";

            Func<IQueryable<GalleryDetail>, IOrderedQueryable<GalleryDetail>> order;
            Expression<Func<GalleryDetail, bool>> filter = x => x.GalleryId == galId;
            var data = new List<GalleryDetail>();
            switch (sortDescriptor.Member)
            {
                case "DisplayOrder":
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.DisplayOrder).ThenBy(z => z.FileName);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.DisplayOrder).ThenBy(z => z.FileName);
                    }
                    break;
                case "FileName":
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.FileName);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.FileName);
                    }
                    break;
                default:
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.Id);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.Id);
                    }
                    break;
            }

            data = _galleryDetailService.Select(filter, order, null, request.Page, request.PageSize).ToList();

            var galleries = new List<GalleryModel>();
            Mapper.CreateMap<GalleryDetail, GalleryModel>();
            galleries = Mapper.Map<List<GalleryDetail>, List<GalleryModel>>(data);

            total = data.Count();
            ViewBag.Total = total;

            var result = new DataSourceResult()
            {
                Data = galleries,
                Total = total
            };
            return Json(result);
        }

        #endregion

        #region #add or update

        public ActionResult Input(int galleryId = 0, int detailId = 0, string redirectUrl = "")
        {
            var entity = _galleryDetailService.Find(detailId);

            if (entity == null)
            {
                entity = new GalleryDetail()
                {
                    DisplayOrder = 1000,
                    IsVideo = false,
                };
            }
            var model = new GalleryModel() { GalleryId = galleryId};

            Mapper.CreateMap<GalleryDetail, GalleryModel>();
            model = Mapper.Map<GalleryDetail, GalleryModel>(entity);
            ViewBag.RedirectUrl = redirectUrl;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Input(GalleryModel model)
        {
            GalleryDetail entity = null;
            try
            {
                if (model.Id > 0)
                    entity = _galleryDetailService.Find(model.Id);
                else
                    entity = new GalleryDetail() {};

                #region Set value for category entity
                entity.FileName = model.FileName;
                entity.GalleryId = model.GalleryId;
                entity.Url = model.Url;
                entity.IsVideo = model.IsVideo;
                entity.DisplayOrder = model.DisplayOrder;
                entity.Thumbnail = model.Thumbnail != null ? model.Thumbnail : "default.jpg";
                #endregion

                #region Perform save data
                if (model.Id <= 0)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        _galleryDetailService.Insert(entity);
                        _unitOfWork.SaveChanges();
                        scope.Complete();
                    }
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        _galleryDetailService.Update(entity);
                        _unitOfWork.SaveChanges();
                        scope.Complete();
                    }
                }

                return Json(new { Status = ResultStatus.Success, Message = StringTable.DataSaveSuccess });
                #endregion
            }
            catch
            {
                return Json(new { Status = ResultStatus.Fail, Message = StringTable.DataSaveUnsuccess });
            }
        }

        #endregion

        #region #delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _galleryDetailService.Find(id);

                if (entity != null)
                {
                    if (!entity.IsVideo)
                    {
                        string pathFile = Globals.ThumbPath(ConstantKeys.GalleryFolder, entity.FileName);
                        var clear = TryDeleteFile(pathFile);
                        if (clear)
                        {
                            pathFile = Globals.ImagePath(ConstantKeys.GalleryFolder, entity.FileName);
                            clear = TryDeleteFile(pathFile);
                        }
                    }
                    _galleryDetailService.Delete(entity);
                    _unitOfWork.SaveChanges();

                    return Json(new { Status = ResultStatus.Success, Message = "Data were deleted successfully!" });
                }

                return Json(new { Status = ResultStatus.Fail, Message = "Data were deleted unsuccessfully!" });
            }
            catch
            {
                return Json(new { Status = ResultStatus.Fail, Message = "Data were deleted unsuccessfully!" });
            }
        }

        #endregion

        #region #others        

        public override ImageUploadModel ImageSetting()
        {
            return new ImageUploadModel()
            {
                ThumbPath = Globals.ThumbPath(ConstantKeys.GalleryFolder),
                ImagePath = Globals.ImagePath(ConstantKeys.GalleryFolder),
                ImageHeight = 600,
                ImageWidth = 800,
                ThumbWidth = 190,
                ThumbHeight = 190,
                Quality = 90,
                AutoResize = false,
                AllowThumb = true,
                Folder = ConstantKeys.GalleryFolder
            };
        }

        #endregion

        #endregion
    }
}
