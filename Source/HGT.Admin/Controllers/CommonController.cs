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


namespace HGT.Admin.Controllers
{
    public class CommonController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IContentService _contentService;

        #endregion

        #region Contructors

        public CommonController(IUnitOfWork unitOfWork, IContentService contentService)
        {
            this._unitOfWork = unitOfWork;
            this._contentService = contentService;
        }

        #endregion

        #region Methods

        public ActionResult Restart()
        {
            HgtCache.Clear();

            TempData["Message"] = "Application restart susscess!";
            TempData.Keep("Message");
            return Redirect("/admin/dashboard");
        }

        #endregion
    }
}
