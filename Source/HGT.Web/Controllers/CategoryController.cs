using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using PagedList;
using System.Net;
using HGT.Core;
using HGT.Core.Paging;
using HGT.Core.Models;
using HGT.Core.Extensions;
using HGT.Core.Enums;
using HGT.Core.Enumerations;
using HGT.Entities.Models;
using HGT.Service;
using HGT.Web.Models;

namespace HGT.Web.Controllers
{
    public class CategoryController : Controller
    {
        #region Cache key
        private const string PRODUCT_ALL_KEY = "Product.All";
        private const string PRODUCT_BY_ALIAS_KEY = "Product.ByAlias-{0}";
        private const string PRODUCT_BY_CATEGORYID_KEY = "Product.ByCategoryId-{0}";
        private const string PRODUCT_GALLERY_PRODUCTID_BY_TYPE_KEY = "Product.Gallery.ByProductIdTypeId-{0}-{1}";

        private const string PRODUCT_ISFEATURE_KEY = "Product.IsFeature";

        private const string CATEGORY_ALL_KEY = "Category.All";
        private const string CATEGORY_BY_ALIAS_KEY = "Category.ByAlias-{0}";
        private const string CATEGORY_GALLERY_PRODUCTID_BY_TYPE_KEY = "Category.Gallery.ByCategoryIdByType-{0}-{1}";

        #endregion

        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IGalleryService _galleryService;
        #endregion

        #region Contructors

        public CategoryController(IUnitOfWork unitOfWork, ICategoryService categoryService, IProductService productService,
            IGalleryService galleryService)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._productService = productService;
            this._galleryService = galleryService;
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {                       
            var model = GetAllProduct();
            return View(model);
        }

        public ActionResult CategoryDetail(string categoryAlias = "")
        {
            if (string.IsNullOrEmpty(categoryAlias))
            {
                return RedirectToAction("Index");
            }

            var categoryKey = string.Format(CATEGORY_BY_ALIAS_KEY, categoryAlias);
            var model = HgtCache.Get<CategoryModel>(categoryKey);
            if (model == null)
            {
                model = new CategoryModel();
                var category = _categoryService.GetCategoryByAlias(categoryAlias);
                if (category != null)
                {
                    model = category.ToModel();

                    #region products
                    // get product list
                    var productKey = string.Format(PRODUCT_BY_CATEGORYID_KEY, model.Id);
                    var products = HgtCache.Get<List<Product>>(productKey);
                    if (products == null)
                    {
                        products = _productService.GetListProductsByCategoryId(model.Id, false);                        
                        HgtCache.Insert(productKey, products);
                    }
                    model.Products = products;

                    #endregion

                    #region gallery

                    // get gallery image
                    var galleryImageKey = string.Format(CATEGORY_GALLERY_PRODUCTID_BY_TYPE_KEY, model.Id, (int)GalleryType.ImageAndVideo);
                    var galleryImage = new List<GalleryDetail>();
                    galleryImage = HgtCache.Get<List<GalleryDetail>>(galleryImageKey);
                    if (galleryImage == null)
                    {
                        galleryImage = _galleryService.GetListGalleryDetailByObjectId(model.Id, (int)GalleryCategory.Category, (int)GalleryType.ImageAndVideo);
                        HgtCache.Insert(galleryImageKey, galleryImage);
                    }
                    model.Galleries = galleryImage;
                    model.TotalGallery = galleryImage.Count;

                    #endregion

                    #region print capabilities

                    // get gallery print
                    var galleryPrintKey = string.Format(CATEGORY_GALLERY_PRODUCTID_BY_TYPE_KEY, model.Id, (int)GalleryType.PrintCapbility);
                    var galleryPrint = new List<GalleryDetail>();
                    galleryPrint = HgtCache.Get<List<GalleryDetail>>(galleryPrintKey);
                    if (galleryPrint == null)
                    {
                        galleryPrint = _galleryService.GetListGalleryDetailByObjectId(model.Id, (int)GalleryCategory.Category, (int)GalleryType.PrintCapbility);
                        HgtCache.Insert(galleryPrintKey, galleryPrint);
                    }
                    model.Prints = galleryPrint;
                    model.TotalPrint = galleryPrint.Count;

                    #endregion
                }
                HgtCache.Insert(categoryKey, model);
            }            
            return View(model);
        }

        public ActionResult ProductDetail(string productAlias)
        {
            if (!string.IsNullOrEmpty(productAlias))
            {
                var productKey = string.Format(PRODUCT_BY_ALIAS_KEY, productAlias);
                var model = new ProductModel();
                model = HgtCache.Get<ProductModel>(productKey);
                if (model == null)
                {
                    var product = _productService.GetProductByAlias(productAlias);

                    if (product != null)
                    {
                        model = product.ToModel();

                        // get category
                        model.Category = _categoryService.GetCategoryById(model.CategoryId);

                        // get gallery images
                        var galleryImageKey = string.Format(PRODUCT_GALLERY_PRODUCTID_BY_TYPE_KEY, model.Id, (int)GalleryType.ImageAndVideo);
                        var galleryImage = new List<GalleryDetail>();
                        galleryImage = HgtCache.Get<List<GalleryDetail>>(galleryImageKey);
                        if (galleryImage == null)
                        {
                            galleryImage = _galleryService.GetListGalleryDetailByObjectId(model.Id, (int)GalleryCategory.Product, (int)GalleryType.ImageAndVideo);
                            HgtCache.Insert(galleryImageKey, galleryImage);
                        }
                        model.Galleries = galleryImage;

                        // get gallery print
                        var galleryPrintKey = string.Format(PRODUCT_GALLERY_PRODUCTID_BY_TYPE_KEY, model.Id, (int)GalleryType.PrintCapbility);
                        var galleryPrint = new List<GalleryDetail>();
                        galleryPrint = HgtCache.Get<List<GalleryDetail>>(galleryPrintKey);
                        if (galleryPrint == null)
                        {
                            galleryPrint = _galleryService.GetListGalleryDetailByObjectId(model.Id, (int)GalleryCategory.Product, (int)GalleryType.PrintCapbility);
                            HgtCache.Insert(galleryPrintKey, galleryPrint);
                        }
                        model.Prints = galleryPrint;

                        // cache
                        HgtCache.Insert(productKey, model);
                    }
                }
                if (model != null)
                    return View(model);
            }
            return HttpNotFound();
        }

        // get list category (left slide menu)
        public ActionResult _ListCategories()
        {
            var model = GetListAllCategory();
            return PartialView(model);
        }

        // get list all category
        public List<Category> GetListAllCategory()
        {
            var model =  HgtCache.Get<List<Category>>(CATEGORY_ALL_KEY);
            if (model == null)
            {
                model = new List<Category>();
                var categories = _categoryService.GetListCategoryByParentId(0).OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name).ToList();
                if (categories != null && categories.Count > 0)
                {
                    //model = model
                    foreach (var item in categories)
                    {
                        var productKey = string.Format(PRODUCT_BY_CATEGORYID_KEY, item.Id);
                        var products = HgtCache.Get<List<Product>>(productKey);
                        if (products == null)
                        {                            
                            products = _productService.GetListProductsByCategoryId(item.Id, false).OrderBy(x=>x.DisplayOrder).ThenBy(x=>x.Name).ToList();
                            products = products.Count > 0 ? products: new List<Product>();
                            HgtCache.Insert(productKey, products);
                        }
                        item.Products = products;
                        model.Add(item);
                        //if (products.Count > 0)
                        //{
                        //    item.Products = products;
                        //    model.Add(item);
                        //}
                    }                    
                }
                HgtCache.Insert(CATEGORY_ALL_KEY, model);
            }
            return model;
        }

        // get list all product
        public List<Product> GetAllProduct()
        {
            var model = HgtCache.Get<List<Product>>(PRODUCT_ALL_KEY);
            if (model == null || model.Count < 1)
            {
                model = new List<Product>();
                var categories = GetListAllCategory();
                if (categories != null)
                {
                    foreach (var category in categories)
                    {
                        var productKey = string.Format(PRODUCT_BY_CATEGORYID_KEY, category.Id);
                        var products = HgtCache.Get<List<Product>>(productKey);
                        if (products == null || products.Count < 1)
                        {
                            products = _productService.GetListProductsByCategoryId(category.Id, false);
                            HgtCache.Insert(productKey, products);
                        }
                        if (products != null && products.Count > 0)
                        {
                            foreach (var product in products)
                            {
                                //var productModel = product.ToModel();
                                //productModel.CategoryAlias = category.Alias;
                                model.Add(product);
                            }
                        }
                    }
                }
                
                HgtCache.Insert(PRODUCT_ALL_KEY, model);
            }

            return model;
        }

        //public ActionResult CategoryDetail1(string categoryAlias = "")
        //{

        //    var productKey = string.Format(ProductCacheKey, categoryAlias);
        //    var products = new List<Product>();


        //    if (string.IsNullOrEmpty(categoryAlias))
        //    {
        //        ViewBag.CategoryName = "Tất cả sản phẩm";
        //        products = HgtCache.Get<List<Product>>(productKey);
        //        if (products == null)
        //        {
        //            products = _productService.ODataQueryable().Where(x => x.IsActive == true).ToList();
        //            HgtCache.Insert(productKey, products);
        //        }
        //    }
        //    else
        //    {
        //        var categoryKey = string.Format(CategoryCacheKey, categoryAlias);
        //        var category = HgtCache.Get<Category>(categoryKey);
        //        if (category == null)
        //        {
        //            category = _categoryService.GetCategoryByAlias(categoryAlias);
        //            HgtCache.Insert(categoryKey, category);
        //        }

        //        if (category != null)
        //        {
        //            ViewBag.CategoryName = category.Name;
        //            products = HgtCache.Get<List<Product>>(productKey);
        //            if (products == null)
        //            {
        //                products = _productService.GetListProductsByCategoryId(category.Id);
        //                HgtCache.Insert(productKey, products);
        //            }
        //        }
        //        else
        //            return HttpNotFound();
        //    }

        //    var pagingSettingModel = new PagingSettingModel(12); ///
        //    var productSettingModel = new ProductSettingModel(pagingSettingModel);

        //    if (products != null)
        //    {
        //        var productModels = new List<ProductViewModel>();
        //        productModels = Mapper.Map<List<Product>, List<ProductViewModel>>(products);

        //        productSettingModel.Total = productModels.Count;
        //        if (productModels.Count > productSettingModel.PageSize)
        //        {
        //            var pagingInfo = new PagingInfo("page");
        //            pagingInfo.TotalItems = productModels.Count;
        //            pagingInfo.CurrentPage = productSettingModel.PageIndex;
        //            pagingInfo.ItemsPerPage = productSettingModel.PageSize;
        //            var url = Request.Url.ToString().TrimEnd('/');
        //            if (!url.Contains("?" + pagingSettingModel.PageIndexQueryKeyName))
        //                url += "?" + pagingSettingModel.PageIndexQueryKeyName + "=1";
        //            pagingInfo.RequestUrl = url;
        //            pagingInfo.PageQueryStringKeyName = pagingSettingModel.PageIndexQueryKeyName;
        //            pagingInfo.ShowAllPages = pagingSettingModel.EnableFullPagingControl;
        //            pagingInfo.PageCount = pagingSettingModel.PagingLength;

        //            productSettingModel.Results = productModels.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.ItemsPerPage).Take(pagingInfo.ItemsPerPage).ToList();
        //            ViewData["PageInfoModel"] = pagingInfo;
        //        }
        //        else
        //            productSettingModel.Results = productModels.ToList();
        //    }

        //    return View(productSettingModel);
        //}


        //public ActionResult QuickSearch(string key, int? page)
        //{
        //    ViewBag.KeySearch = key;
        //    var pagingSettingModel = new PagingSettingModel(12);/////
        //    var productSettingModel = new ProductSettingModel(pagingSettingModel);

        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        var products = _productService.ODataQueryable().Where(x => x.Name.ToLower().Contains(key.ToLower())).ToList();

        //        if (products != null)
        //        {
        //            var productModels = new List<ProductViewModel>();
        //            productModels = Mapper.Map<List<Product>, List<ProductViewModel>>(products);

        //            productSettingModel.Total = productModels.Count;
        //            if (productModels.Count > productSettingModel.PageSize)
        //            {
        //                var pagingInfo = new PagingInfo("page");
        //                pagingInfo.TotalItems = productModels.Count;
        //                pagingInfo.CurrentPage = productSettingModel.PageIndex;
        //                pagingInfo.ItemsPerPage = productSettingModel.PageSize;
        //                var url = Request.Url.ToString().TrimEnd('/');
        //                if (!url.Contains("?" + pagingSettingModel.PageIndexQueryKeyName))
        //                    url += "?" + pagingSettingModel.PageIndexQueryKeyName + "=1";
        //                pagingInfo.RequestUrl = url;
        //                pagingInfo.PageQueryStringKeyName = pagingSettingModel.PageIndexQueryKeyName;
        //                pagingInfo.ShowAllPages = pagingSettingModel.EnableFullPagingControl;
        //                pagingInfo.PageCount = pagingSettingModel.PagingLength;

        //                productSettingModel.Results = productModels.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.ItemsPerPage).Take(pagingInfo.ItemsPerPage).ToList();
        //                ViewData["PageInfoModel"] = pagingInfo;
        //            }
        //            else
        //                productSettingModel.Results = productModels.ToList();
        //        }
        //    }

        //    return View(productSettingModel);
        //}

        //public ActionResult _ListProductsSameCategory(int categoryId, int productId)
        //{
        //    var productModels = new List<ProductViewModel>();
        //    var products = _productService.GetListProductsSameCategory(categoryId, productId);

        //    if (products != null && products.Count > 0)
        //        productModels = Mapper.Map<List<Product>, List<ProductViewModel>>(products);
        //    return PartialView(productModels);
        //}

        //// get list category show home page
        //public ActionResult _ListProductsShowIndex()
        //{
        //    var productModels = new List<ProductViewModel>();
        //    var products = new List<Product>();

        //    products = HgtCache.Get<List<Product>>(PRODUCT_ISFEATURE_KEY);
        //    if (products == null)
        //    {
        //        products = _productService.GetListProductsIsHighlight(8);
        //        HgtCache.Insert(PRODUCT_ISFEATURE_KEY, products);
        //    }

        //    if (products != null && products.Count > 0)
        //    {
        //        productModels = Mapper.Map<List<Product>, List<ProductViewModel>>(products);
        //    }

        //    return PartialView(productModels);
        //}



        //// get list categories (nav menu)
        //public ActionResult _NavMenuCategories()
        //{
        //    var categories = _categoryService.GetListCategoryByParentId(0).Take(5).ToList();
        //    if (categories == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var categoryModels = Mapper.Map<List<Category>, List<CategoryViewModel>>(categories);

        //    return PartialView(categoryModels);
        //}

        #endregion
    }
}



//var ctx = ClawContext.Current;
//var kw = Globals.GenerateAlias(Query.Keyword) + Query.Keyword.GetHashCode();
//var cacheKey = string.Format("ClawSearch-Lang:{0}-IsMobileDevice:{1}-Keyword:{2}", ctx.CurrentLanguage.LanguageCode,
//                                ctx.IsMobileDevice, kw);

//var result = ClawCache.Get<SearchCollection>(cacheKey);

//// no search result in cache
//if (result == null || result.Count == 0)
//{
//    result = new SearchCollection();
//    var mdlList = ctx.DataService.GetAll<DM.Module>(ClawConfiguration.GetConfig().SiteID);

//    if (mdlList != null)
//        mdlList = mdlList.Where(a => a.Active && a.IsSearchable).ToList();

//    if (mdlList != null && mdlList.Count > 0)
//    {
//        foreach (var mdl in mdlList)
//        {
//            var path = string.Format("~/bin/{0}", mdl.DLLName);
//            path = Globals.MapPath(path);

//            if (!File.Exists(path))
//                continue;

//            foreach (DM.Component comp in mdl.Modulecms_Componentses)
//            {
//                if (comp.Active && comp.IsSearchable)
//                {
//                    var tmpResult = this.SearchByComponent(comp);
//                    result.AddRange(tmpResult);
//                }
//            }
//        }

//        ClawCache.Insert(cacheKey, result, CacheTimout);
//    }
//}