using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Core.Utility
{
    public class UrlUtility
    {                
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

        public static string MiddleTruncate(string url, int desireLength, bool includeProtocol = true)
        {
            if (string.IsNullOrEmpty(url) || url.Length <= desireLength + 3)
            {
                return url;
            }
            var uriBuilder = new UriBuilder(url);

            var domain = "";
            if (includeProtocol)
            {
                domain = uriBuilder.Port != 80 ? string.Format("{0}://{1}:{2}", uriBuilder.Scheme, uriBuilder.Host, uriBuilder.Port)
                    : string.Format("{0}://{1}", uriBuilder.Scheme, uriBuilder.Host);
            }
            else
            {
                domain = uriBuilder.Port != 80 ? string.Format("{0}:{1}", uriBuilder.Host, uriBuilder.Port) : uriBuilder.Host;
            }

            if (domain.Length >= desireLength - 3 && domain.Length <= desireLength && uriBuilder.Uri.PathAndQuery.Length > 3)
            {
                return domain + "/...";
            }
            else if (domain.Length > desireLength)//Test
            {
                return domain.Substring(0, desireLength) + "...";
            }

            // =>less
            var rest = "";
            var remainLength = desireLength - domain.Length;
            var pathAndQuery = uriBuilder.Uri.PathAndQuery;
            if (!string.IsNullOrEmpty(pathAndQuery) && pathAndQuery.Length > remainLength)
            {
                var i = 1;
                var nextSlash = 0;
                do
                {
                    nextSlash = pathAndQuery.IndexOf("/", i);
                    if (nextSlash > 0)
                    {
                        i = nextSlash + 1;
                    }
                } while (nextSlash < remainLength / 3 && nextSlash > 0);

                if (i > 0 && remainLength - i >= i)// && remainLength - nextSlash > remainLength / 3)
                {
                    rest = pathAndQuery.Substring(0, nextSlash + 1);

                    var removeLength = pathAndQuery.Length - (remainLength - rest.Length);
                    rest += "..." + pathAndQuery.Remove(0, removeLength);
                }

                else if (nextSlash > 0)
                {
                    var removeLength = pathAndQuery.Length - remainLength;
                    rest = string.Format("/...{0}", pathAndQuery.Remove(0, removeLength));
                }
                else
                {
                    var removeLength = pathAndQuery.Length - remainLength;
                    rest = string.Format("...{0}", pathAndQuery.Remove(0, removeLength));
                }
            }
            else
            {
                rest = pathAndQuery;
            }
            return string.Format("{0}{1}", domain, rest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPaths">The virtual paths.<example>string[] {"path1","path2","path3"}</example></param>
        /// <returns> <value>path1/path2/path3</value> </returns>
        public static string Combine(params string[] virtualPaths)
        {
            if (virtualPaths.Length < 1)
                return null;

            return RawCombine(virtualPaths.Select(it => it.Contains("/") ? it : Uri.EscapeUriString(it)).ToArray());
        }

        /// <summary>
        /// Combines the specified virtual paths without Escape.
        /// </summary>
        /// <param name="virtualPaths">The virtual paths.</param>
        /// <returns></returns>
        public static string RawCombine(params string[] virtualPaths)
        {
            if (virtualPaths.Length < 1)
                return null;

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < virtualPaths.Length; i++)
            {
                var path = virtualPaths[i];
                if (String.IsNullOrEmpty(path))
                    continue;

                path = path.Replace("\\", "/");

                if (i > 0)
                {
                    // Not first one trim start '/'
                    path = path.TrimStart('/');
                    builder.Append("/");
                }
                if (i < virtualPaths.Length - 1)
                {
                    // Not last one trim end '/'
                    path = path.TrimEnd('/');
                }
                builder.Append(path);
            }

            return builder.ToString();
        }
    }
}
