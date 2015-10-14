using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.IO;
using Kendo.Mvc.UI;
using Kendo.Mvc;
using System.ComponentModel;
using System.Linq.Expressions;
using Newtonsoft.Json;
using HGT.Entities.Models;
using HGT.Core;
using HGT.Core.Enums;
using HGT.Core.Providers;
using HGT.Core.Utility;
using HGT.Core.Extensions;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using HGT.Admin.Models;
using HGT.Core.Models;
using System.Transactions;

namespace HGT.Admin.Controllers
{
    public class BannerController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBannerService _bannerService;

        private UserInfo userInfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;
        #endregion

        #region Contructors

        public BannerController(IUnitOfWork unitOfWork, IBannerService bannerService)
        {
            this._unitOfWork = unitOfWork;
            this._bannerService = bannerService;
        }

        #endregion

        #region Methods

        #region #index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            int _total = 0;
            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("Id", ListSortDirection.Ascending);

            sortDescriptor.Member = sortDescriptor.Member ?? "Title";
            Func<IQueryable<Banner>, IOrderedQueryable<Banner>> order;
           
            var data = new List<Banner>();
            switch (sortDescriptor.Member)
            {
                case "Title":
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.Title);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.Title);
                    }
                    break;
                case "DisplayOrder":
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.DisplayOrder).ThenBy(z => z.Title);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.DisplayOrder).ThenBy(z => z.Title);
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

            data = _bannerService.Select(null, order, null, request.Page, request.PageSize).ToList();

            var banners = data.ToListModel();

            _total = _bannerService.Select(null, order, null, null, null).Count();
            ViewBag.Total = _total;
            var result = new DataSourceResult()
            {
                Data = banners,
                Total = _total
            };
            return Json(result);
        }

        #endregion

        #region #add or update

        public ActionResult Input(int id = 0)
        {
            var entity = _bannerService.Find(id);

            if (entity == null)
            {
                entity = new Banner()
                {
                    DisplayOrder = 1000,
                    IsActive = true
                };
            }
            var model = entity.ToModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Input(BannerModel model)
        {
            if (ModelState.IsValid)
            {
                Banner entity = null;
                try
                {
                    if (model.Id > 0)
                        entity = _bannerService.Find(model.Id);
                    else
                        entity = new Banner();

                    #region Set value for category entity
                   
                    entity.Title = model.Title;
                    entity.Description = model.Description;
                    entity.Link = model.Link;
                    entity.OpenLink = model.OpenLink;
                    entity.FileName = model.FileName != null ? model.FileName : "default.jpg";
                    entity.IsActive = model.IsActive;
                    entity.DisplayOrder = model.DisplayOrder;

                    #endregion

                    #region Perform save data
                    if (model.Id <= 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _bannerService.Insert(entity);
                            _unitOfWork.SaveChanges();
                            scope.Complete();
                        }
                    }
                    else
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _bannerService.Update(entity);
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
            else
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
                var entity = _bannerService.Find(id);

                if (entity != null)
                {
                    _bannerService.Delete(entity);;
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
                ThumbPath = Globals.ThumbPath(ConstantKeys.BannerFolder),
                ImagePath = Globals.ImagePath(ConstantKeys.BannerFolder),
                ImageWidth = 550,
                ImageHeight = 450,
                ThumbWidth = 110,
                ThumbHeight = 90,
                Quality = 100,
                AutoResize = true,
                AllowThumb = false,
                Folder = ConstantKeys.BannerFolder
            };
        }

        #endregion

        #endregion
    }
}