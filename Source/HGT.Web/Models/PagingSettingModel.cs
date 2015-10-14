using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HGT.Web.Models
{
    public class PagingSettingModel
    {
        public string KeywordQueryKeyName { get; set; }

        public string PageIndexQueryKeyName { get; set; }

        public string PageSizeQueryKeyName { get; set; }

        public int PageSize { get; set; }
       
        public bool EnableFullPagingControl { get; set; }

        public int PagingLength { get; set; }
        
        public PagingSettingModel(int pageSize)
        {
            this.KeywordQueryKeyName = "kw";
            this.PageIndexQueryKeyName = "p";
            this.PageSizeQueryKeyName = "pz";
            this.PageSize = pageSize;
            this.EnableFullPagingControl = true;
            this.PagingLength = 5;
        }    
    }
}
