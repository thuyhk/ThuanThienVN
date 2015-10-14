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

namespace HGT.Admin.Controllers
{
    public class CategoryController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IGalleryService _galleryService;
        private readonly IGalleryDetailService _galleryDetailService;

        private UserInfo _userInfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;
        #endregion

        #region Contructors

        public CategoryController(IUnitOfWork unitOfWork, ICategoryService categoryService,
            IProductService productService, IUserService userService,
            ISettingService settingService, IGalleryService galleryService, IGalleryDetailService galleryDetailService)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._productService = productService;
            this._userService = userService;
            this._settingService = settingService;
            this._galleryService = galleryService;
            this._galleryDetailService = galleryDetailService;
        }

        #endregion

        #region Methods

        #region category setting

        public ActionResult Setting()
        {
            var model = new CategorySettingsModel();
            var setting = _settingService.Find((int)SettingType.CategorySetting);
            if (setting != null)
            {
                model = SerializationUtility.DeserializeFromXml<CategorySettingsModel>(setting.Values);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string EditSetting(CategorySettingsModel settingModel)
        {
            if (ModelState.IsValid)
            {
                var setting = _settingService.Find(settingModel.Id);
                if (setting != null)
                {
                    setting.Values = SerializationUtility.SerializeAsXml(settingModel);
                    _settingService.Update(setting);
                    _unitOfWork.SaveChanges();

                    return JsonConvert.SerializeObject(new { Status = 1, Message = "Update success." });
                }
                return JsonConvert.SerializeObject(new { Status = 0, Message = "Update unsuccess." });
            }
            return JsonConvert.SerializeObject(new { Status = 0, Message = "Update unsuccess." });
        }

        #endregion

        #region #search

        public ActionResult Filter()
        {
            string searchValue = Request.Params["filter[filters][0][value]"];
            if (!string.IsNullOrEmpty(searchValue))
            {
                var entity = _categoryService.GetSearchKey(searchValue).Take(50).ToList();
                var model = entity.ToListModel();
                return Json(entity, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        #endregion

        #region #index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request, string keyword = null)
        {
            int total = 0;
            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("DisplayOrder", ListSortDirection.Ascending);

            sortDescriptor.Member = sortDescriptor.Member ?? "DisplayOrder";
            Func<IQueryable<Category>, IOrderedQueryable<Category>> order;
            Expression<Func<Category, bool>> filter = x => x.Id > 0;

            //Expression<Func<YourEntity, bool>> FilterByNameLength(int length) { return x => x.Name.Length > length;}
            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x => x.Name.Contains(keyword);
            }
            var data = new List<Category>();
            switch (sortDescriptor.Member)
            {
                case "Name":
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.Name);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.Name);
                    }
                    break;
                default:
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.DisplayOrder).ThenBy(z => z.Name);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.DisplayOrder).ThenBy(z => z.Name);
                    }
                    break;
            }

            data = _categoryService.Select(filter, order, null, request.Page, request.PageSize).ToList();

            var categories = data.ToListModel();

            foreach (var category in categories)
            {
                category.TotalGallery = _galleryService.GetListGalleryDetailByObjectId(category.Id, (int)GalleryCategory.Category, (int)GalleryType.ImageAndVideo).Count;
                category.TotalPrint = _galleryService.GetListGalleryDetailByObjectId(category.Id, (int)GalleryCategory.Category, (int)GalleryType.PrintCapbility).Count;
            }

            total = _categoryService.Select(filter, order, null, null, null).Count();
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
            var category = _categoryService.Find(id);

            if (category == null)
            {
                category = new Category()
                {
                    DisplayOrder = 1000,
                    IsActive = true,
                    IsFeature = true,
                    EnablePrintCapbility = true,
                    EnableGallery = true
                };
            }
            var model = category.ToModel();
            var categories = GetListCategory();
            if (id > 0)
                categories = categories.Where(x => x.Value != id.ToString()).ToList();
            ViewData["ListCategory"] = categories;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Input(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                Category category = null;
                try
                {
                    if (model.Id > 0)
                        category = _categoryService.Find(model.Id);
                    else
                        category = new Category() { CreatedDate = DateTime.Now };

                    #region Set value for category entity
                    category.Name = model.Name;
                    category.Alias = model.Alias;
                    category.Brief = model.Brief;
                    category.Description = model.Description;
                    category.IsActive = model.IsActive;
                    category.IsFeature = model.IsFeature;
                    category.DisplayOrder = model.DisplayOrder;
                    category.Thumbnail = model.Thumbnail != null ? model.Thumbnail : "default.jpg";
                    category.RootId = model.RootId;//
                    //category.HowIt = model.HowIt;
                    //category.KeyBenefit = model.KeyBenefit;
                    //category.Solution = model.Solution;
                    category.EnableGallery = model.EnableGallery;
                    //category.EnablePrintCapbility = model.EnablePrintCapbility;
                    category.UpdatedDate = DateTime.Now;
                    #endregion

                    #region Perform save data
                    if (model.Id <= 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _categoryService.Insert(category);
                            _unitOfWork.SaveChanges();
                            scope.Complete();
                        }
                    }
                    else
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _categoryService.Update(category);
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

        #region #gallery

        public ActionResult GalleryDetail(int categoryId = 0, string redirectUrl = "")
        {
            var category = _categoryService.Find(categoryId);
            if (category == null)
                return RedirectToAction("Index");

            var gallery = _galleryService.GetGalleryByObject(categoryId, (int)GalleryCategory.Category, (int)GalleryType.ImageAndVideo);

            if (gallery == null)
            {
                gallery = new Gallery()
                {
                    ObjectId = categoryId,
                    Category = (int)GalleryCategory.Category,
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
                var entity = _categoryService.Find(id);

                if (entity != null)
                {
                    // get list category by parent
                    var lstCategoryId = new List<int>();
                    lstCategoryId.Add(entity.Id);

                    var categoryChilds1 = _categoryService.GetListCategoryByParentId(entity.Id);
                    if (categoryChilds1.Count > 0)
                    {
                        foreach (var item1 in categoryChilds1)
                        {
                            lstCategoryId.Add(item1.Id);
                            var categoryChilds2 = _categoryService.GetListCategoryByParentId(item1.Id);
                            if (categoryChilds2.Count > 0)
                            {
                                foreach (var item2 in categoryChilds2)
                                {
                                    lstCategoryId.Add(item2.Id);
                                }
                            }
                        }
                    }

                    lstCategoryId = lstCategoryId.Distinct().ToList();
                    foreach (var itemId in lstCategoryId)
                    {
                        //  clear gallery
                        var galleries = _galleryService.GetListGalleryByObjectId(entity.Id, (int)GalleryCategory.Category);
                        if (galleries != null && galleries.Count > 0)
                        {
                            foreach (var gallery in galleries)
                            {
                                if (gallery != null)
                                {
                                    var galleryDetails = _galleryService.GetListGalleryDetailByGalleryId(gallery.Id);
                                    if (galleryDetails != null && galleryDetails.Count > 0)
                                    {
                                        foreach (var galleryDetail in galleryDetails)
                                        {
                                            _galleryDetailService.Delete(galleryDetail);
                                        }
                                    }
                                    _galleryService.Delete(gallery);
                                }
                            }
                        }

                        // update product mapping
                        var products = _productService.GetListProductsByCategoryId(itemId);
                        foreach (var item in products)
                        {
                            item.CategoryId = 0;
                            item.ObjectState = ObjectState.Modified;
                            _productService.Update(item);
                        }
                        var category = _categoryService.GetCategoryById(itemId);
                        _categoryService.Delete(category);
                    }
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

        #region ValidateAlias

        [HttpPost]
        public JsonResult ValidateAlias(string alias, int? id)
        {
            var retCode = _categoryService.ValidateAlias(alias, id);
            return Json(retCode, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected List<SelectListItem> GetListCategory()
        {
            var listCategory = new List<SelectListItem>();
            listCategory.Add(new SelectListItem() { Text = "None", Value = "0" });
            foreach (var itemL1 in _categoryService.GetListCategory(0))
            {
                listCategory.Add(new SelectListItem() { Text = itemL1.Name, Value = itemL1.Id.ToString() });
                var getCateChildByItemL1 = _categoryService.GetListCategory(itemL1.Id);
                if (getCateChildByItemL1.Count > 0)
                {
                    foreach (var itemL2 in getCateChildByItemL1)
                    {
                        listCategory.Add(new SelectListItem() { Text = "--- " + itemL2.Name, Value = itemL2.Id.ToString() });
                        var getCateChildByItemL2 = _categoryService.GetListCategory(itemL2.Id);
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
                ThumbPath = Globals.ThumbPath(ConstantKeys.ProductFolder),
                ImagePath = Globals.ImagePath(ConstantKeys.ProductFolder),
                ImageWidth = 800,
                ImageHeight = 600,
                ThumbWidth = 272,
                ThumbHeight = 186,
                Quality = 100,
                AutoResize = true,
                AllowThumb = true,
                Folder = ConstantKeys.ProductFolder
            };
        }

        #endregion

        #endregion
    }
}
