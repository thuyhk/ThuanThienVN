using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using HGT.Web.Extensions;

namespace HGT.Web.Controllers
{
    public class ProductController : Controller
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        #endregion

        #region Contructors

        public ProductController(IUnitOfWork unitOfWork, ICategoryService categoryService, IProductService productService)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._productService = productService;
        }

        #endregion

        #region Methods


        #endregion
    }
}