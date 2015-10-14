using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGT.Web.Models
{
    public class PageSizeModel
    {
        public int ShowIndex { get; set; }
        public int CategoryPageSize { get; set; }
        public int NewPageSize { get; set; }
    }
}