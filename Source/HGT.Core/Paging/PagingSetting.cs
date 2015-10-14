using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Core.Paging
{
    public class PagingSetting
    {
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public string OriginalUrl { get; set; }

        public int TotalPages
        {
            get
            {
                if (PageSize <= 0 || Total <= 0)
                    return 0;
                return (int)Math.Ceiling((decimal)Total / PageSize);
            }
        }
    }
}
