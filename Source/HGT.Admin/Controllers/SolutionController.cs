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
using AutoMapper;
using Newtonsoft.Json;
using System.Drawing;
using System.Transactions;
using HGT.Entities.Models;
using HGT.Core;
using HGT.Core.Enums;
using HGT.Core.Models;
using HGT.Core.Extensions;
using HGT.Core.Utility;
using HGT.Service;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using HGT.Admin.Models;
using HGT.Core.Enumerations;
using HGT.Admin.Extensions;

namespace HGT.Admin.Controllers
{
    public class SolutionController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISolutionService _solutionService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IGalleryService _galleryService;
        private readonly IGalleryDetailService _galleryDetailService;
        private readonly ISolutionProductService _solutionProductService;

        private UserInfo _userInfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;
        #endregion

        #region Contructors

        public SolutionController(IUnitOfWork unitOfWork, ISolutionService solutionService,
            IProductService productService, IUserService userService,
            ISettingService settingService, IGalleryService galleryService, IGalleryDetailService galleryDetailService,
            ISolutionProductService solutionProductService)
        {
            this._unitOfWork = unitOfWork;
            this._solutionService = solutionService;
            this._productService = productService;
            this._userService = userService;
            this._settingService = settingService;
            this._galleryService = galleryService;
            this._galleryDetailService = galleryDetailService;
            this._solutionProductService = solutionProductService;
        }

        #endregion

        #region Methods

        #region #index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request, string keyword = null)
        {
            int total = 0;
            var query = _solutionService.ODataQueryable().ToList();

            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("DisplayOrder", ListSortDirection.Ascending);
            var asc = sortDescriptor.SortDirection.Equals(ListSortDirection.Ascending) ? true : false;
            Func<Solution, object> selector = x => x.Name;

            switch (sortDescriptor.Member)
            {
                case "DisplayOrder":
                    selector = x => x.DisplayOrder;
                    break;
                case "RootId":
                    selector = x => x.RootId;
                    break;
            }

            var data = query.Order(selector, asc).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).ToList();

            var categories = data.ToListModel();

            foreach (var category in categories)
            {
                category.TotalGallery = _galleryService.GetListGalleryDetailByObjectId(category.Id, (int)GalleryCategory.Category, (int)GalleryType.ImageAndVideo).Count;
            }

            total = query.Count();
            ViewBag.Total = total;
            var result = new DataSourceResult()
            {
                Data = categories,
                Total = total
            };
            return Json(result);
        }

        #endregion

        #region #add or update

        public ActionResult Input(int id = 0)
        {
            var solution = _solutionService.Find(id);

            if (solution == null)
            {
                solution = new Solution()
                {
                    DisplayOrder = 1000,
                    IsActive = true,
                    IsFeature = true,
                    EnableGallery = true
                };
            }
            var model = solution.ToModel();
            var solutions = GetListCategory();
            if (id > 0)
                solutions = solutions.Where(x => x.Value != id.ToString()).ToList();
            ViewData["ListSolution"] = solutions;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Input(SolutionModel model)
        {
            if (ModelState.IsValid)
            {
                Solution solution = null;
                try
                {
                    if (model.Id > 0)
                        solution = _solutionService.Find(model.Id);
                    else
                        solution = new Solution();

                    #region Set value for category entity
                    solution.Name = model.Name;
                    solution.Alias = model.Alias;
                    solution.Brief = model.Brief;
                    solution.Description = model.Description;
                    solution.IsActive = model.IsActive;
                    solution.IsFeature = model.IsFeature;
                    solution.DisplayOrder = model.DisplayOrder;
                    solution.Thumbnail = model.Thumbnail != null ? model.Thumbnail : "default.jpg";
                    solution.RootId = model.RootId;//                  
                    solution.EnableGallery = model.EnableGallery;
                    solution.OpenLink = model.OpenLink;
                    solution.Link = model.Link;
                    #endregion

                    #region Perform save data
                    if (model.Id <= 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _solutionService.Insert(solution);
                            _unitOfWork.SaveChanges();
                            scope.Complete();
                        }
                    }
                    else
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _solutionService.Update(solution);
                            _unitOfWork.SaveChanges();
                            scope.Complete();
                        }
                    }

                    return Json(new { Status = ResultStatus.Success, Message = "Data were saved successfully!" });
                    #endregion
                }
                catch
                {
                    return Json(new { Status = ResultStatus.Fail, Message = StringTable.DataSaveUnsuccess });
                }
            }
            else
            {
                return Json(new { Status = ResultStatus.Fail, Message = StringTable.DataSaveSuccess });
            }
        }

        #endregion

        #region #gallery

        public ActionResult GalleryDetail(int solutionId = 0, string redirectUrl = "")
        {
            var category = _solutionService.Find(solutionId);
            if (category == null)
                return RedirectToAction("Index");

            var gallery = _galleryService.GetGalleryByObject(solutionId, (int)GalleryCategory.Solution, (int)GalleryType.ImageAndVideo);

            if (gallery == null)
            {
                gallery = new Gallery()
                {
                    ObjectId = solutionId,
                    Category = (int)GalleryCategory.Solution,
                    CreatedDate = DateTime.Now,
                    Name = category.Name,
                    Type = (int)GalleryType.ImageAndVideo,
                    Alias = category.Alias
                };
                _galleryService.Insert(gallery);
                _unitOfWork.SaveChanges();
            }

            return RedirectToAction("Index", "Gallery", new { galleryId = gallery != null ? gallery.Id : 0, redirectUrl = redirectUrl });
        }

        #endregion

        #region #delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _solutionService.Find(id);

                if (entity != null)
                {
                    var details = _solutionProductService.ODataQueryable().Where(x => x.SolutionId == entity.Id).ToList();
                    if (details.Count > 0)
                    {
                        foreach (var item in details)
                        {
                            _solutionProductService.Delete(item);
                        }
                    }
                    _solutionService.Delete(entity);
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

        //[HttpPost]
        //public JsonResult CheckCategoryExist(string categoryAlias, int categoryId)
        //{
        //    var model = _categoryService.CheckCategoryExist(categoryAlias, categoryId);
        //    return Json(model);
        //}


        #region ValidateAlias

        [HttpPost]
        public JsonResult ValidateAlias(string alias, int? id)
        {
            var retCode = _solutionService.ValidateAlias(alias, id);
            return Json(retCode, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected List<SelectListItem> GetListCategory()
        {
            var listCategory = new List<SelectListItem>();
            listCategory.Add(new SelectListItem() { Text = "None", Value = "0" });
            foreach (var itemL1 in _solutionService.GetListSolutions(0))
            {
                listCategory.Add(new SelectListItem() { Text = itemL1.Name, Value = itemL1.Id.ToString() });
                var getCateChildByItemL1 = _solutionService.GetListSolutions(itemL1.Id);
                if (getCateChildByItemL1.Count > 0)
                {
                    foreach (var itemL2 in getCateChildByItemL1)
                    {
                        listCategory.Add(new SelectListItem() { Text = "--- " + itemL2.Name, Value = itemL2.Id.ToString() });
                        var getCateChildByItemL2 = _solutionService.GetListSolutions(itemL2.Id);
                        if (getCateChildByItemL2.Count > 0)
                        {
                            foreach (var itemL3 in getCateChildByItemL1)
                            {
                                listCategory.Add(new SelectListItem() { Text = "------ " + itemL3.Name, Value = itemL3.Id.ToString() });
                            }
                        }
                    }
                }

            }
            return listCategory;
        }

        public override ImageUploadModel ImageSetting()
        {
            return new ImageUploadModel()
            {
                ThumbPath = Globals.ThumbPath(ConstantKeys.SolutionFolder),
                ImagePath = Globals.ImagePath(ConstantKeys.SolutionFolder),
                ImageWidth = 800,
                ImageHeight = 600,
                ThumbWidth = 272,
                ThumbHeight = 186,
                Quality = 80,
                AutoResize = true,
                AllowThumb = true,
                Folder = ConstantKeys.SolutionFolder
            };
        }

        #endregion

        #endregion
    }
}
