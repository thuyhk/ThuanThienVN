using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using HGT.Web.Extensions;
using PagedList;
using HGT.Core.Enums;
using HGT.Entities.Models;
using HGT.Core.Models;
using HGT.Web.Models;
using AutoMapper;
using HGT.Core.Paging;
using System.Net;
using HGT.Core.Enumerations;

namespace HGT.Web.Controllers
{
    public class NewsController : Controller
    {
        #region Properties

        private readonly IUnitOfWork _unitOfWork;
        private readonly INewsService _newService;

        private readonly WebHelper _webHelper = new HGT.Web.Extensions.WebHelper();

        #endregion

        #region Contructors

        public NewsController(IUnitOfWork unitOfWork, INewsService newService)
        {
            this._unitOfWork = unitOfWork;
            this._newService = newService;
        }

        #endregion

        #region Methods

        public ActionResult ListSupport()
        {
            var news = new List<News>();
            news = _newService.GetListNewsByCategoryId((int)CategoryNews.Support);

            var pagingSettingModel = new PagingSettingModel(12); ////
            var model = NewsPading(news, pagingSettingModel);

            return View(model);
        }

        public NewsPagingModel NewsPading(List<News> news, PagingSettingModel pagingSettingModel)
        {
            var newsSettingModel = new NewsPagingModel(pagingSettingModel);

            if (news != null)
            {
                newsSettingModel.Total = news.Count;
                if (news.Count > newsSettingModel.PageSize)
                {
                    var pagingInfo = new PagingInfo("page");
                    pagingInfo.TotalItems = news.Count;
                    pagingInfo.CurrentPage = newsSettingModel.PageIndex;
                    pagingInfo.ItemsPerPage = newsSettingModel.PageSize;
                    var url = Request.Url.ToString().TrimEnd('/');
                    if (!url.Contains("?" + pagingSettingModel.PageIndexQueryKeyName))
                        url += "?" + pagingSettingModel.PageIndexQueryKeyName + "=1";
                    pagingInfo.RequestUrl = url;
                    pagingInfo.PageQueryStringKeyName = pagingSettingModel.PageIndexQueryKeyName;
                    pagingInfo.ShowAllPages = pagingSettingModel.EnableFullPagingControl;
                    pagingInfo.PageCount = pagingSettingModel.PagingLength;

                    newsSettingModel.Results = news.Skip((pagingInfo.CurrentPage - 1) * pagingInfo.ItemsPerPage).Take(pagingInfo.ItemsPerPage).ToList();
                    ViewData["PageInfoModel"] = pagingInfo;
                }
                else
                    newsSettingModel.Results = news.ToList();                
            }
            return newsSettingModel;
        }

        public ActionResult Detail(string alias)
        {
            var entity = _newService.GetNewsByAlias(alias);
            if (entity == null)
                return HttpNotFound();
            else
            {
                if (entity.CategoryId.Equals((int)CategoryNews.News))
                    ViewBag.CategoryLink = "<a href='/tin-tuc' title='tin tức'>Tin tức</a>";
                else ViewBag.CategoryLink = "<a href='/tu-van-ho-tro' title='Hỗ trợ kỹ thuật'>Hỗ trợ kỹ thuật</a>";
                return View(entity);
            }
        }

        public ActionResult _ListOTherNews(int categoryId, int newId)
        {
            var _models = new List<News>();
            _models = _newService.GetListOtherNews(0, newId);
            return PartialView(_models);
        }

        public ActionResult _ListTopNews()
        {
            var _models = new List<News>();
            _models = _newService.GetListTopNews(0);
            return PartialView(_models);
        }

        #endregion
    }
}