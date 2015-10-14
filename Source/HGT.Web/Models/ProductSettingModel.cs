using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HGT.Core.Paging;
using HGT.Core.Models;

namespace HGT.Web.Models
{
    public class ProductSettingModel : PagingSetting
    {
        public List<Product> Results { get; set; }

        public ProductSettingModel(PagingSettingModel setting)
        {
            var uriBuilder = new UriBuilder(HttpContext.Current.Request.Url.ToString());
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (query != null && setting != null)
            {
                this.Keyword = query[setting.KeywordQueryKeyName];
                var pageIndex = !string.IsNullOrEmpty(setting.PageIndexQueryKeyName) ? query[setting.PageIndexQueryKeyName] : string.Empty;
                if (!string.IsNullOrEmpty(pageIndex))
                {
                    query.Remove(setting.PageIndexQueryKeyName);
                    uriBuilder.Query = query.ToString();
                    this.OriginalUrl = uriBuilder.Uri.PathAndQuery;
                }
                else
                    this.OriginalUrl = HttpContext.Current.Request.RawUrl;

                var pageSize = !string.IsNullOrEmpty(setting.PageSizeQueryKeyName) && !string.IsNullOrEmpty(query[setting.PageSizeQueryKeyName]) ? query[setting.PageSizeQueryKeyName] : setting.PageSize.ToString();
                int index = 0, size = 0;
                int.TryParse(pageIndex, out index);
                this.PageIndex = index > 0 ? index : 1;
                int.TryParse(pageSize, out size);
                this.PageSize = size > 0 ? size : 1;
            }
        }

    }
}