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
using HGT.Core.Enumerations;


namespace HGT.Admin.Controllers
{
    public class NewsController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newsService;

        #endregion

        #region Contructors

        public NewsController(IUnitOfWork unitOfWork, INewsService newsService)
        {
            this._unitOfWork = unitOfWork;
            this._newsService = newsService;
        }

        #endregion

        #region Methods

        #region #search

        public ActionResult Filter()
        {
            string searchValue = Request.Params["filter[filters][0][value]"];
            if (!string.IsNullOrEmpty(searchValue))
            {
                var list = _newsService.GetSearchKey(searchValue).Take(50);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        #endregion

        #region #index
        public ActionResult Index(int categoryId = 0)
        {
            var categories = new List<SelectListItem>();
            categories.Add(new SelectListItem() { Text = "All category", Value = "0" });
            foreach (var item in Enum.GetValues(typeof(CategoryNews)))
            {
                categories.Add(new SelectListItem() { Text = item.ToString(), Value = ((int)item).ToString() });
            }

            ViewData["CategoryId"] = categoryId.ToString();
            ViewData["ListCategory"] = categories;
            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request, string keyword = null, int categoryId = 0)
        {
            int total = 0;
            SortDescriptor sortDescriptor = (request.Sorts != null && request.Sorts.Count > 0) ? request.Sorts.FirstOrDefault() : new SortDescriptor("CreatedDate", ListSortDirection.Descending);

            sortDescriptor.Member = sortDescriptor.Member ?? "Title";
            Func<IQueryable<News>, IOrderedQueryable<News>> order;
            Expression<Func<News, bool>> filter = x => x.Id > 0;
            if (!string.IsNullOrEmpty(keyword))
            {
                filter = x => x.Title.Contains(keyword);
            }

            if (categoryId > 0)
            {
                filter = x => x.CategoryId.Equals(categoryId);
            }

            var data = new List<News>();
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

            data = _newsService.Select(filter, order, null, request.Page, request.PageSize).ToList();
            total = _newsService.Select(filter, order, null, null, null).Count();
            ViewBag.Total = total;
            var result = new DataSourceResult()
            {
                Data = data,
                Total = total
            };
            return Json(result);
        }

        #endregion     

        #region #delete

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var entity = _newsService.Find(id);

                if (entity != null)
                {
                    _newsService.Delete(entity);
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

        //check category name exist
        [HttpPost]
        public JsonResult CheckNewsTitleExist(string name, int id)
        {
            var user = _newsService.CheckNewsTitleExist(name, id);
            return Json(user);
        }

        #endregion

        #endregion
    }
}
