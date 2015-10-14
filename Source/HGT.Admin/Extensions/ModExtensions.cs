using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Admin.Extensions
{
    public static class ModExtensions
    {

        #region EqualsIgCase
        /// <summary>
        ///  Equals Ignore Case
        /// </summary>
        public static bool EqualsIgCase(this string value, string target)
        {
            if (value == null) return target == null;
            else return value.Equals(target, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region CombineWPath

        public static string CombineWPath(this string value, string path)
        {
            if (string.IsNullOrEmpty(value)) return path;

            if (value.EndsWith("/")) return value + path;
            else return value + '/' + path;
        }

        #endregion
    }
}
