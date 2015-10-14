using HGT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HGT.Core.Paging;
using HGT.Core.Utility;


namespace HGT.Web.Extensions
{
    public class SiteControl
    {
        public SiteControl()
        {
        }

        public MvcHtmlString PageLinks(PagingInfo pagingInfo)
        {
            if (pagingInfo.TotalPages <= 1)
                return new MvcHtmlString("");

            #region recheck page info

            var defaultInfo = new PagingInfo();
            if (pagingInfo.ItemsPerPage <= 0)
                pagingInfo.ItemsPerPage = defaultInfo.ItemsPerPage;

            if (pagingInfo.PageCount <= 0)
                pagingInfo.PageCount = defaultInfo.PageCount;

            if (pagingInfo.CurrentPage <= 0 || pagingInfo.CurrentPage > pagingInfo.TotalPages)
                pagingInfo.CurrentPage = 1;

            var startPage = 0;
            if (pagingInfo.CurrentPage % pagingInfo.PageCount == 0)
                startPage = (int)(pagingInfo.CurrentPage / pagingInfo.PageCount) * pagingInfo.PageCount - pagingInfo.PageCount + 1;
            else
                startPage = (int)(pagingInfo.CurrentPage / pagingInfo.PageCount) * pagingInfo.PageCount + 1;

            var endPage = startPage + pagingInfo.PageCount - 1;
            if (endPage > pagingInfo.TotalPages)
                endPage = pagingInfo.TotalPages;

            #endregion

            var result = new StringBuilder();

            if (pagingInfo.ShowAllPages)
            {
                #region Enable full paging

                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    var tag = new TagBuilder("a");
                    var cssClass = "";

                    if (i == 1)
                        cssClass = !string.IsNullOrEmpty(pagingInfo.FirstPageNumberCssClass) ? pagingInfo.FirstPageNumberCssClass : defaultInfo.FirstPageNumberCssClass;

                    else if (i == pagingInfo.CurrentPage)
                        cssClass = !string.IsNullOrEmpty(pagingInfo.CurrentPageCssClass) ? pagingInfo.CurrentPageCssClass : defaultInfo.CurrentPageCssClass;

                    else
                        cssClass = !string.IsNullOrEmpty(pagingInfo.PageNumberCssClass) ? pagingInfo.PageNumberCssClass : defaultInfo.PageNumberCssClass;

                    tag.MergeAttribute("href", AddQueryString(pagingInfo.RequestUrl, pagingInfo.PageQueryStringKeyName, i.ToString()));
                    tag.InnerHtml = i.ToString();
                    tag.AddCssClass(cssClass);

                    result.Append(tag.ToString());
                }

                #endregion
            }
            else
            {
                #region show paging with current paging length settings

                for (int i = startPage; i <= endPage; i++)
                {
                    var tag = new TagBuilder("a");
                    var cssClass = "";

                    if (i == 1)
                        cssClass = !string.IsNullOrEmpty(pagingInfo.FirstPageNumberCssClass) ? pagingInfo.FirstPageNumberCssClass : defaultInfo.FirstPageNumberCssClass;

                    else if (i == pagingInfo.CurrentPage)
                        cssClass = !string.IsNullOrEmpty(pagingInfo.CurrentPageCssClass) ? pagingInfo.CurrentPageCssClass : defaultInfo.CurrentPageCssClass;

                    else
                        cssClass = !string.IsNullOrEmpty(pagingInfo.PageNumberCssClass) ? pagingInfo.PageNumberCssClass : defaultInfo.PageNumberCssClass;

                    tag.MergeAttribute("href", AddQueryString(pagingInfo.RequestUrl, pagingInfo.PageQueryStringKeyName, i.ToString()));
                    tag.InnerHtml = i.ToString();
                    tag.AddCssClass(cssClass);

                    result.Append(tag.ToString());
                }

                #endregion
            }

            var controlBuilder = new StringBuilder();

            #region show first/preview/compact link

            #region  Show first Page

            if (pagingInfo.CurrentPage > 1 && pagingInfo.ShowFirstLast)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(defaultInfo.FirstPageLinkCssClass);
                if (!string.IsNullOrEmpty(pagingInfo.FirstPageLinkCssClass) && string.Compare(defaultInfo.FirstPageLinkCssClass, pagingInfo.FirstPageLinkCssClass, true) != 0)
                {
                    tag.AddCssClass(pagingInfo.FirstPageLinkCssClass);
                }

                tag.MergeAttribute("href", AddQueryString(pagingInfo.RequestUrl, pagingInfo.PageQueryStringKeyName, "1"));
                tag.InnerHtml = "&lsaquo;";//.Localize("First");

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            #region Show previous Page

            if (pagingInfo.CurrentPage > 1 && pagingInfo.ShowNextPrevious)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(defaultInfo.PrePageCssClass);
                if (!string.IsNullOrEmpty(pagingInfo.PrePageCssClass) && string.Compare(defaultInfo.PrePageCssClass, pagingInfo.PrePageCssClass, true) != 0)
                {
                    tag.AddCssClass(pagingInfo.PrePageCssClass);
                }

                tag.MergeAttribute("href", AddQueryString(pagingInfo.RequestUrl, pagingInfo.PageQueryStringKeyName, (pagingInfo.CurrentPage - 1).ToString()));
                tag.InnerHtml = "&laquo;";//.Localize("Previous");

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            #region show compact link

            if (pagingInfo.ShowCompactLink && startPage > 1)
            {
                var tag = new TagBuilder("a");
                var css = !string.IsNullOrEmpty(pagingInfo.CompactPageLinkCssClass) ? pagingInfo.CompactPageLinkCssClass : defaultInfo.FirstPageLinkCssClass;
                var pageIndex = (startPage - 1).ToString();

                tag.AddCssClass(css);
                tag.MergeAttribute("href", AddQueryString(pagingInfo.RequestUrl, pagingInfo.PageQueryStringKeyName, pageIndex));
                tag.InnerHtml = "...";//.Localize("...");

                controlBuilder.Append(tag.ToString());
            }

            if (controlBuilder.Length > 0)
                result.Insert(0, controlBuilder);

            #endregion

            #endregion

            #region show compact/next/last link

            controlBuilder = new StringBuilder();

            #region show compact link

            if (pagingInfo.ShowCompactLink && endPage < pagingInfo.TotalPages)
            {
                var tag = new TagBuilder("a");
                var css = !string.IsNullOrEmpty(pagingInfo.CompactPageLinkCssClass) ? pagingInfo.CompactPageLinkCssClass : defaultInfo.FirstPageLinkCssClass;
                var pageIndex = (endPage + 1).ToString();

                tag.AddCssClass(css);
                tag.MergeAttribute("href", AddQueryString(pagingInfo.RequestUrl, pagingInfo.PageQueryStringKeyName, pageIndex));
                tag.InnerHtml = "...";//.Localize("...");

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            #region Show next Page

            if (pagingInfo.CurrentPage < pagingInfo.TotalPages && pagingInfo.ShowNextPrevious)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(defaultInfo.NextPageCssClass);
                if (!string.IsNullOrEmpty(pagingInfo.NextPageCssClass) && string.Compare(defaultInfo.NextPageCssClass, pagingInfo.NextPageCssClass, true) != 0)
                {
                    tag.AddCssClass(pagingInfo.NextPageCssClass);
                }

                tag.MergeAttribute("href", AddQueryString(pagingInfo.RequestUrl, pagingInfo.PageQueryStringKeyName, (pagingInfo.CurrentPage + 1).ToString()));
                tag.InnerHtml = "&rsaquo;";//.Localize("Next");

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            #region Show last Page

            if (pagingInfo.CurrentPage < pagingInfo.TotalPages && pagingInfo.ShowFirstLast)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(defaultInfo.LastPageLinkCssClass);
                if (!string.IsNullOrEmpty(pagingInfo.LastPageLinkCssClass) && string.Compare(defaultInfo.LastPageLinkCssClass, pagingInfo.LastPageLinkCssClass, true) != 0)
                {
                    tag.AddCssClass(pagingInfo.LastPageLinkCssClass);
                }

                tag.MergeAttribute("href", AddQueryString(pagingInfo.RequestUrl, pagingInfo.PageQueryStringKeyName, pagingInfo.TotalPages.ToString()));
                tag.InnerHtml = "&raquo";//.Localize("Last");

                controlBuilder.Append(tag.ToString());
            }

            #endregion

            if (controlBuilder.Length > 0)
                result.Append(controlBuilder);

            #endregion

            #region show summary Info

            if (pagingInfo.ShowSummaryInfo)
            {
                var infoTemplate = pagingInfo.SummaryInfoTemplate;
                infoTemplate = !string.IsNullOrEmpty(infoTemplate) ? infoTemplate : ""; //"Trang {0}/{1} ({2} items)";

                var summary = string.Format(infoTemplate, pagingInfo.CurrentPage, pagingInfo.TotalPages, pagingInfo.TotalItems);
                result.Insert(0, summary);
            }

            #endregion

            return MvcHtmlString.Create(result.ToString());
        }

        private static string AddQueryString(string url, string name, string value)
        {
            var uriBuilder = new UriBuilder(url);
            var query = UrlUtility.ParseQueryString(uriBuilder.Query);
            query[name] = value;
            uriBuilder.Query = UrlUtility.ToQueryString(query);
            return uriBuilder.Uri.PathAndQuery;
        }
    }
}