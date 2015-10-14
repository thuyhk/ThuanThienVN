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
using HGT.Core.Models;
using HGT.Core.Extensions;
using HGT.Core.Utility;
using HGT.Core.Enums;
using HGT.Core.Enumerations;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using HGT.Admin.Models;

namespace HGT.Admin.Controllers
{
    public class ProductController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        private readonly IGalleryService _galleryService;
        private readonly IGalleryDetailService _galleryDetailService;

        #endregion

        #region Contructors

        public ProductController(IUnitOfWork unitOfWork, ICategoryService categoryService,
            IProductService productService, IUserService userService,
            ISettingService settingService, IGalleryService galleryService,
            IGalleryDetailService galleryDetailService)
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

        #region product setting

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
                var entity = _productService.GetSearchKey(searchValue).Take(50).ToList();
                var model = entity.ToListModel();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        #endregion

        #region #index
        public ActionResult Index(int categoryId = 0)
        {
            var categories = new List<SelectListItem>();
            categories.Add(new SelectListItem() { Text = "All category", Value = "0" });
            categories.AddRange(GetListCategory());

            ViewData["CategoryId"] = categoryId.ToString();
            ViewData["ListCategory"] = categories;
            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request, string keyword = null, int categoryId = 0)
        {
            int total = 0;
            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("DisplayOrder", ListSortDirection.Ascending);

            sortDescriptor.Member = sortDescriptor.Member ?? "DisplayOrder";
            Func<IQueryable<Product>, IOrderedQueryable<Product>> order;
            Expression<Func<Product, bool>> filter = x => x.IsActive == true;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x => x.Name.Contains(keyword);
            }

            if (categoryId > 0)
            {
                filter = x => x.CategoryId.Equals(categoryId);
            }

            var data = new List<Product>();
            switch (sortDescriptor.Member)
            {
                case "CategoryId":
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.CategoryId).ThenBy(z => z.Name);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.CategoryId).ThenBy(z => z.Name);
                    }
                    break;
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
                case "CreatedDate":
                    if (sortDescriptor.SortDirection == ListSortDirection.Ascending)
                    {
                        order = x => x.OrderBy(y => y.CreatedDate);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.CreatedDate);
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

            data = _productService.Select(filter, order, null, request.Page, request.PageSize).ToList();

            var products = data.ToListModel();

            foreach (var product in products)
            {
                product.TotalGallery = _galleryService.GetListGalleryDetailByObjectId(product.Id, (int)GalleryCategory.Product, (int)GalleryType.ImageAndVideo).Count;
            }

            total = _productService.Select(filter, order, null, null, null).Count();
            ViewBag.Total = total;
            var result = new DataSourceResult()
            {
                Data = products,
                Total = total
            };
            return Json(result);
        }

        #endregion

        #region #add or update

        public ActionResult Input(int id = 0)
        {
            var entity = _productService.Find(id);

            if (entity == null)
            {
                entity = new Product()
                {
                    IsActive = true,
                    EnableGallery = true
                };
            }
            var model = entity.ToModel();
            ViewData["ListCategory"] = GetListCategory();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Input(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                Product entity = null;
                try
                {
                    if (model.Id > 0)
                        entity = _productService.Find(model.Id);
                    else
                        entity = new Product() { CreatedDate = DateTime.Now };

                    #region Set value for category entity

                    entity.SKU = model.SKU;
                    entity.Name = model.Name;
                    entity.Alias = model.Alias;
                    entity.Brief = model.Brief;
                    entity.Description = model.Description;
                    entity.Price = entity.Price;
                    entity.IsActive = model.IsActive;
                    entity.DisplayOrder = model.DisplayOrder;
                    entity.Thumbnail = model.Thumbnail != null ? model.Thumbnail : "default.jpg";
                    entity.CategoryId = model.CategoryId;
                    entity.CallPrice = model.CallPrice;
                    entity.Capability = model.Capability;
                    entity.KeyBenefit = model.KeyBenefit;
                    entity.CaseStudy = model.CaseStudy;
                    entity.EnableGallery = model.EnableGallery;
                    entity.UpdatedDate = DateTime.Now;
                    entity.UpdatedBy = User.Identity.Name;
                    if (!string.IsNullOrEmpty(model.FileAttachTemp))
                        entity.FileAttach = model.FileAttachTemp;
                    entity.FileAttachName = model.FileAttachName;

                    #endregion

                    #region Perform save data
                    if (model.Id <= 0)
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _productService.Insert(entity);
                            _unitOfWork.SaveChanges();
                            scope.Complete();
                        }
                    }
                    else
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            _productService.Update(entity);
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

        public ActionResult GalleryDetail(int productId = 0, string redirectUrl = "")
        {
            var product = _productService.Find(productId);
            if (product == null)
                return RedirectToAction("Index");

            var gallery = _galleryService.GetGalleryByObject(productId, (int)GalleryCategory.Product, (int)GalleryType.ImageAndVideo);

            if (gallery == null)
            {
                gallery = new Gallery()
                {
                    ObjectId = productId,
                    Category = (int)GalleryCategory.Product,
                    CreatedDate = DateTime.Now,
                    Name = product.Name,
                    Alias = product.Alias,
                    Type = (int)GalleryType.ImageAndVideo
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
                var entity = _productService.Find(id);

                if (entity != null)
                {
                    //  clear gallery
                    var galleries = _galleryService.GetListGalleryByObjectId(entity.Id, (int)GalleryCategory.Product);
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

                    // clear product mapping

                    _productService.Delete(entity);
                    _unitOfWork.SaveChanges();

                    return Json(new { Status = ResultStatus.Success, Message = StringTable.DataDeletedSuccess });
                }

                return Json(new { Status = ResultStatus.Fail, Message = StringTable.DataDeletedUnsuccess });
            }
            catch
            {
                return Json(new { Status = ResultStatus.Fail, Message = StringTable.DataDeletedUnsuccess });
            }
        }

        #endregion

        #region #others

        #region ValidateAlias

        [HttpPost]
        public JsonResult ValidateAlias(string alias, int? id)
        {
            var retCode = _productService.ValidateAlias(alias, id);
            return Json(retCode, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected List<SelectListItem> GetListCategory()
        {
            var listCategory = new List<SelectListItem>();
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
                ImageWidth = 400,
                ImageHeight = 400,
                ThumbWidth = 272,
                ThumbHeight = 272,
                Quality = 80,
                AutoResize = true,
                AllowThumb = true,
                Folder = ConstantKeys.ProductFolder
            };
        }

        #endregion

        #endregion
    }
}
