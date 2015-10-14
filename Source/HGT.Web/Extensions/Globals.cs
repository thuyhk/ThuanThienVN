using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace HGT.Web.Extensions
{
    public class Globals
    {
        #region String utilities

        /// <summary>
        /// Resolves the server URL from client url.
        /// </summary>
        /// <param name="RelativePath">The client url.</param>
        /// <returns></returns>
        public static string ResolveUrl(string clientUrl)
        {
            if (clientUrl.StartsWith("/"))
                return string.Format("{0}{1}", "~", clientUrl);
            else
                return string.Format("{0}{1}", "~/", clientUrl);
        }

        /// <summary>
        /// Resolves the client URL from Server relative path (Start with ~/ by the real path).
        /// </summary>
        /// <param name="RelativePath">The relative path.</param>
        /// <returns></returns>
        public static string ResolveClientUrl(string relativePath)
        {
            string appPath = GetApplicationPath();

            if (string.IsNullOrEmpty(relativePath))
                return appPath;

            if (relativePath.StartsWith("/"))
                return relativePath;

            else if (relativePath.StartsWith("~/"))
                relativePath = relativePath.TrimStart(new char[] { '~', '/' });

            return string.Format("{0}{1}", appPath, relativePath);
        }


        /// <summary>
        /// Gets the application virtual path (include Slash at the end, example: /, /CatenoCMS/).
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationPath()
        {
            string AppPath = System.Web.HttpRuntime.AppDomainAppVirtualPath;
            if (!AppPath.EndsWith("/"))
                AppPath += "/";
            return AppPath;
        }

        #endregion

        #region Querystring utiliies

        /// <summary>
        /// Gets the query string value from url or query string
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="queryKey">The query key.</param>
        /// <returns></returns>
        public static string GetQueryStringValue(string url, string queryKey)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            var arr = url.Split(new string[] { "?" }, StringSplitOptions.RemoveEmptyEntries);
            var queryString = arr.Length == 2 ? arr[1] : (arr != null ? arr[0] : "");

            var arrkv = queryString.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            if (arrkv != null && arrkv.Length > 0)
            {
                foreach (var kv in arrkv)
                {
                    var tmpkv = kv.Split('=');
                    if (tmpkv != null && tmpkv.Length == 2 &&
                        string.Compare(tmpkv[0], queryKey) == 0)
                    {
                        return tmpkv[1];
                    }
                }
            }

            return string.Empty;
        }

        public static NameValueCollection ParseQueryString(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }
            if (query.StartsWith("?"))
            {
                query = query.TrimStart(new[] { '?', '/' });
            }

            var pairs = query.Split(new[] { '&' });

            var result = new NameValueCollection();
            if (pairs.Length > 0)
            {
                foreach (var s in pairs)
                {
                    var pair = s.Split(new[] { '=' });//, StringSplitOptions.RemoveEmptyEntries);
                    if (!string.IsNullOrEmpty(pair[0]))
                    {
                        result.Add(pair[0], pair[1]);
                    }
                }
            }
            return result;
        }

        public static string ToQueryString(NameValueCollection nameValueCollection)
        {
            var result = "";
            if (nameValueCollection != null && nameValueCollection.Count > 0)
            {
                foreach (string key in nameValueCollection.Keys)
                {
                    result += string.Format("{0}={1}&", key, nameValueCollection[key]);
                }

                result = result.TrimEnd(new[] { '&' });
            }
            return result;
        }

        #endregion

    }
}