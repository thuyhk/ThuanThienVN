using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Admin.Extensions
{
    public static class OrderExtensions
    {
        public static IEnumerable<T> Order<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, bool asc)
        {
            return asc ? source.OrderBy(selector) : source.OrderByDescending(selector); 
        }
    }
}
