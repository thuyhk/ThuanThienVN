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
using Newtonsoft.Json;
using HGT.Core.Utility;
using HGT.Core.Extensions;


namespace HGT.Admin.Controllers
{
    public class ProductController1 : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        private UserInfo _userInfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;
        //private AdminExtensions _helper = new AdminExtensions();

        private static LogoModel _logoModel = new LogoModel();

        #endregion

        #region Contructors

        public ProductController(IUnitOfWork unitOfWork, ICategoryService categoryService, IProductService productService, IUserService userService)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._productService = productService;
            this._userService = userService;
        }

        #endregion

        #region Methods

        #region #search

        public ActionResult Filter()
        {
            string searchValue = Request.Params["filter[filters][0][value]"];
            if (!string.IsNullOrEmpty(searchValue))
            {
                var list = _productService.GetSearchKey(searchValue).Take(50);
                return Json(list, JsonRequestBehavior.AllowGet);
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
            int _total = 0;
            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("CreatedDate", ListSortDirection.Descending);

            sortDescriptor.Member = sortDescriptor.Member ?? "Name";
            Func<IQueryable<Product>, IOrderedQueryable<Product>> order;
            Expression<Func<Product, bool>> filter = x => x.IsActive == true;
            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x => x.Name.Contains(keyword);
            }
            var data = new List<Product>();
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
                        order = x => x.OrderBy(y => y.CreatedDate);
                    }
                    else
                    {
                        order = x => x.OrderByDescending(y => y.CreatedDate);
                    }
                    break;
            }

            data = _productService.Select(filter, order, null, request.Page, request.PageSize).ToList();

            var products = new List<ProductModel>();
            AutoMapper.Mapper.CreateMap<Product, ProductModel>();
            products = AutoMapper.Mapper.Map<List<Product>, List<ProductModel>>(data);

            _total = _productService.Select(filter, order, null, null, null).Count();
            ViewBag.Total = _total;
            var result = new DataSourceResult()
            {
                Data = products,
                Total = _total
            };
            return Json(result);
        }

        #endregion

        #region #detail
        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new CategoryModel1();
            //model = _categoryService.Find(id).ToModel();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region #create

        public ActionResult Create()
        {
            var model = new ProductModel();
            model.ListCategories = GetListCategory();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedDate = DateTime.Now;
                    model.UpdatedDate = DateTime.Now;
                    model.CreatedBy = User.Identity.Name;
                    model.UpdatedBy = User.Identity.Name;
                    model.IsActive = true;
                    model.Alias = Globals.GenerateAlias(model.Name, true);

                    #region upload file image to server

                    try
                    {
                        if (_logoModel != null && !string.IsNullOrEmpty(_logoModel.FileName))
                        {
                            model.Picture = _logoModel.FileName;
                            var srcFile = Path.Combine(Server.MapPath(Globals.TempImagePath(_logoModel.FileName)));
                            var desFile = Path.Combine(Server.MapPath(Globals.ImagePath(ConstantKeys.ProductFolder, _logoModel.FileName)));

                            // delete image if exist
                            IOUtility.DeleteFile(desFile);

                            // get temp file
                            string tempFile = Server.MapPath(Globals.TempImagePath(_logoModel.FileName));

                            // get folder thumbs
                            string thumbFolder = Server.MapPath(Globals.ThumbPath(ConstantKeys.ProductFolder, _logoModel.FileName));

                            // get folder image
                            string imageFolder = Server.MapPath(Globals.ImagePath(ConstantKeys.ProductFolder, _logoModel.FileName));

                            // create thumb image
                            System.Drawing.Image img = ImageUtility.ScaleImage(tempFile, 208, 180, "#ffffff");
                            ImageUtility.SaveImage(thumbFolder, img);
                            img.Dispose();

                            // create image
                            img = ImageUtility.ScaleImage(tempFile, 200, 200, "#ffffff");
                            ImageUtility.SaveImage(imageFolder, img);
                            img.Dispose();

                            // delete temp file
                            IOUtility.DeleteFile(srcFile);

                            //// move full images
                            //IOUtility.MoveFile(srcFile, desFile);
                        }
                        else
                            model.Picture = "default.jpg";
                        _logoModel = null;
                    }
                    catch (Exception ex)
                    {
                        model.Picture = "default.jpg";
                        ex.ToString();
                    }

                    #endregion

                    var entity = model.ToEntity();
                    entity.ObjectState = ObjectState.Added;
                    _productService.Insert(entity);
                    _unitOfWork.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region #edit

        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var model = _productService.Find(id).ToModel();
            if (model == null)
                return RedirectToAction("AccessDeny", "Role");
            foreach (var item in _categoryService.GetListCategory(null))
                model.ListCategories.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString(), Selected = (item.Id == model.CategoryId) });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Edit(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _productService.Find(model.Id);
                if (entity != null)
                {
                    entity.UpdatedDate = DateTime.Now;
                    if (entity.Name != model.Name)
                    {
                        entity.Name = model.Name;
                        entity.Alias = Globals.GenerateAlias(model.Name, true);
                    }
                    if (entity.Brief != model.Description)
                        entity.Brief = model.Description;
                    if (entity.Description != model.Content)
                        entity.Description = model.Content;
                    if (entity.CategoryId != model.CategoryId)
                        entity.CategoryId = model.CategoryId;
                    if (entity.Price != model.Price)
                        entity.Price = model.Price;
                    if (entity.Unit != model.Unit)
                        entity.Unit = model.Unit;
                    if (entity.IsActive != model.IsActive)
                        entity.IsActive = model.IsActive;
                    //if (entity.IsFeature != model.ShowIndex)
                    //    entity.IsFeature = model.ShowIndex;
                    if (entity.Origin != model.Origin)
                        entity.Origin = model.Origin;
                    if (entity.CallPrice != model.CallPrice)
                        entity.CallPrice = model.CallPrice;

                    #region upload image

                    if (_logoModel != null && !string.IsNullOrEmpty(_logoModel.FileName))
                    {
                        //delete file old
                        if (entity.Thumbnail != "default.jpg")
                        {
                            var pathOld = Globals.ImagePath(entity.Thumbnail);
                            IOUtility.DeleteFile(pathOld);
                        }

                        entity.Thumbnail = _logoModel.FileName;
                        var srcFile = Path.Combine(Server.MapPath(Globals.TempImagePath(_logoModel.FileName)));
                        var desFile = Path.Combine(Server.MapPath(Globals.ImagePath(ConstantKeys.ProductFolder, _logoModel.FileName)));

                        // delete image if exist
                        IOUtility.DeleteFile(desFile);

                        // get temp file
                        string tempFile = Server.MapPath(Globals.TempImagePath(_logoModel.FileName));

                        // get folder thumbs
                        string thumbFolder = Server.MapPath(Globals.ThumbPath(ConstantKeys.ProductFolder, _logoModel.FileName));

                        // get folder image
                        string imageFolder = Server.MapPath(Globals.ImagePath(ConstantKeys.ProductFolder, _logoModel.FileName));

                        // create thumb image
                        System.Drawing.Image img = ImageUtility.ScaleImage(tempFile, 208, 180, "#ffffff");
                        ImageUtility.SaveImage(thumbFolder, img);
                        img.Dispose();

                        // create image
                        img = ImageUtility.ScaleImage(tempFile, 200, 200, "#ffffff");
                        ImageUtility.SaveImage(imageFolder, img);
                        img.Dispose();

                        // delete temp file
                        IOUtility.DeleteFile(srcFile);

                        //// move full images
                        //IOUtility.MoveFile(srcFile, desFile);                       
                    }
                    _logoModel = null;

                    #endregion

                    entity.ObjectState = ObjectState.Modified;
                    _productService.Update(entity);
                    _unitOfWork.SaveChanges();
                }
                return JsonConvert.SerializeObject(new { Status = 1, Message = "Update success." });
            }
            else
            {
                return JsonConvert.SerializeObject(new { Status = 0, Message = "Update unsuccess." });
            }
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
                    //delete data
                    _productService.Delete(entity);
                    //entity.IsActive = false;
                    //_productService.Update(entity);
                    _unitOfWork.SaveChanges();

                    return Json(new { Status = 1, Message = "Delete success!" });
                }

                return Json(new { Status = 0, Message = "Delete error!" });
            }
            catch
            {
                return Json(new { Status = 0, Message = "Delete error!" });
            }
        }

        #endregion

        #region #others

        //upload file : auto save avatar
        public ActionResult SaveLogoUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (_logoModel != null)
                {
                    DeleteTempFile(_logoModel.FileName);
                    _logoModel = null;
                }

                // Upload file in to UploadFolder
                var fileName = ImageUtility.RandomString() + Path.GetFileName(file.FileName);
                var pathFile = Globals.TempImagePath(fileName);
                var physhicalPath = Path.Combine(Server.MapPath(pathFile));
                file.SaveAs(physhicalPath);

                _logoModel = new LogoModel() { FileName = fileName, PictureUrl = pathFile };
                file.SaveAs(physhicalPath);

            }
            return Json(_logoModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveLogoUpload()
        {
            DeleteTempFile(_logoModel.FileName);
            _logoModel = null;
            return Json("");

        }

        private void DeleteTempFile(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var physicalPath = Path.Combine(Server.MapPath(Globals.TempImagePath(filename)));
                IOUtility.DeleteFile(physicalPath);
            }
        }

        //check category name exist
        [HttpPost]
        public JsonResult CheckProductExist(string name, int id)
        {
            var user = _productService.CheckProductNameExist(name, id);
            return Json(user);
        }


        private List<SelectListItem> GetListCategory()
        {
            var listCategory = new List<SelectListItem>();            
            //foreach (var itemL1 in _categoryService.GetListCategoryByParentId(0))
            //{
            //    listCategory.Add(new SelectListItem() { Text = itemL1.Name, Value = itemL1.Id.ToString() });
            //    var itemModel1 = itemL1.ToModel();
            //    if (itemModel1.Childrens != null && itemModel1.Childrens.Count > 0)
            //    {
            //        foreach (var itemL2 in itemModel1.Childrens)
            //        {
            //            if (listCategory.Where(x => x.Value == itemL2.Id.ToString()).FirstOrDefault() == null)
            //            {
            //                listCategory.Add(new SelectListItem() { Text = "----- " + itemL2.Name, Value = itemL2.Id.ToString() });
            //                var itemModel2 = itemL2.ToModel();
            //                if (itemModel2.Childrens != null && itemModel2.Childrens.Count > 0)
            //                {
            //                    foreach (var itemL3 in itemModel2.Childrens)
            //                    {
            //                        listCategory.Add(new SelectListItem() { Text = "---------- " + itemL3.Name, Value = itemL3.Id.ToString() });
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}
            return listCategory;
        }

        #endregion

        #endregion
    }
}
