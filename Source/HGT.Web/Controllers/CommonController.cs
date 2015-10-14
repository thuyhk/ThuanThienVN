using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HGT.Entities.Models;
using HGT.Service;
using HGT.Web.Extensions;
using HGT.Web.Models;
using Repository.Pattern.UnitOfWork;
using PagedList;
using HGT.Core.Enums;

namespace HGT.Web.Controllers
{
    public class CommonController : Controller
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IStoreService _storeService;
        private readonly IContentService _contentService;

        #endregion

        #region Contructors

        public CommonController(IUnitOfWork unitOfWork, ICategoryService categoryService, IStoreService storeService,
            IContentService contentService)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._storeService = storeService;
            this._contentService = contentService;
        }

        #endregion

        #region Methods

        #region #common layout

        public ActionResult _QuickSearch()
        {
            return PartialView();
        }

        public ActionResult _NavMenu()
        {
            return PartialView();
        }

        public ActionResult _FooterInfo()
        {
            return PartialView();
        }

        public ActionResult _Footer()
        {
            ViewData["ContactOSS"] = _contentService.Find((int)ContentType.Footer).Value;
            return PartialView();
        }

        #endregion

        #region #page store

        public ActionResult _ListStoresShowHome()
        {
            var _lstStores = new List<Store>();
            _lstStores = _storeService.GetListStores(8);
            return PartialView(_lstStores);
        }

        public ActionResult StoreDetail(string Alias)
        {
            var _store = new Store();
            _store = _storeService.GetStoreByAlias(Alias);
            return View(_store);
        }

        #endregion

        #endregion
    }
}