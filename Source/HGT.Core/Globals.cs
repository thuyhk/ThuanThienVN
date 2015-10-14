using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HGT.Core
{
    public class Globals
    {
        #region Redirect/Valid Url

        // the HTML newline character
        public const String HtmlNewLine = "<br />";
        public const String _appSettingsPrefix = "AspNetForumsSettings.";
        static Regex validProtocols = new Regex("^(?:https?:|/)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidHttpUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;
            else
                return validProtocols.IsMatch(url);
        }

        public static void RedirectToSSL(HttpContext context)
        {
            if (!context.Request.IsSecureConnection)
            {
                Uri url = context.Request.Url;
                context.Response.Redirect("https://" + url.ToString().Substring(7));
            }
        }

        /// <summary>
        /// Will return false if no Url querystring value can be found
        /// </summary>
        /// <returns></returns>
        public static bool RedirectSiteUrl()
        {

            return false;
        }

        #endregion

        #region Encode/Decode

        public static string EncodeToUtf8(string unicodeString)
        {
            if (string.IsNullOrEmpty(unicodeString))
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] encodedBytes = utf8.GetBytes(unicodeString);
                return Encoding.UTF8.GetString(encodedBytes);
            }

            return unicodeString;
        }

        public static string DecodeUtf8ToUnicode(string uft8String)
        {
            if (string.IsNullOrEmpty(uft8String))
            {
                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                byte[] encodedBytes = unicodeEncoding.GetBytes(uft8String);
                return Encoding.Unicode.GetString(encodedBytes);
            }

            return uft8String;
        }

        public static string HtmlDecode(string textToFormat)
        {
            if (!string.IsNullOrEmpty(textToFormat))
                return HttpUtility.HtmlDecode(textToFormat);

            return textToFormat;
        }

        public static string HtmlEncode(string textToFormat)
        {
            if (!string.IsNullOrEmpty(textToFormat))
                return HttpUtility.HtmlEncode(textToFormat);

            return textToFormat;
        }

        public static string UrlEncode(string urlToEncode)
        {
            if (!string.IsNullOrEmpty(urlToEncode))
                return HttpUtility.UrlEncode(urlToEncode);

            return urlToEncode;
        }

        public static string UrlDecode(string urlToDecode)
        {
            if (!string.IsNullOrEmpty(urlToDecode))
                return HttpUtility.UrlDecode(urlToDecode);

            return urlToDecode;
        }

        public static string EmailUrlEncode(string email)
        {
            string result = email;

            if (!string.IsNullOrEmpty(result))
            {
                result = string.Empty;
                Random rnd = new Random();
                char[] chars = email.ToCharArray();

                for (var j = 0; j < chars.Length; j++)
                {
                    if (rnd.Next(100) <= 50)
                        result = result + "%" + Convert.ToString((int)chars[j], 16);
                    else
                        result = result + Convert.ToString(chars[j]);
                }
            }

            return result;
        }

        public static string EmailHtmlEncode(string email)
        {
            string result = email;

            if (!string.IsNullOrEmpty(result))
            {
                result = string.Empty;
                Random rnd = new Random();

                char[] emailChars = email.ToCharArray();
                for (var j = 0; j < emailChars.Length; j++)
                {
                    if (rnd.Next(100) <= 50 || emailChars[j] == ':' || emailChars[j] == '@' || emailChars[j] == '.')
                    {
                        result = result + "&#" + Convert.ToString((int)emailChars[j]) + ";";
                    }
                    else
                        result = result + Convert.ToString(emailChars[j]);
                }
            }
            return result;
        }

        public static string EmailNumberCode(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                StringBuilder result = new StringBuilder();
                char[] emailChars = email.ToCharArray();

                for (var j = 0; j < emailChars.Length; j++)
                {
                    result.AppendFormat("{0},", (byte)emailChars[j]);
                }

                return result.ToString().TrimEnd(',');
            }
            return email;
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

        /// <summary>
        /// Adds a string to the current url determining the correct seperator character. Ex: queryString value: "page=1&pagesize=10&keyword=test"
        /// </summary>
        /// <param name="url"></param>
        /// <param name="querystring"></param>
        /// <returns></returns>
        public static string AppendQueryString(string url, string querystring)
        {
            if (string.IsNullOrEmpty(querystring))
                return url;

            var arrQuery = querystring.Split('&');
            if (arrQuery.Length > 0)
            {
                foreach (var query in arrQuery)
                {
                    var arrKeyValue = query.Split('=');
                    if (arrKeyValue.Length == 2)
                        url = AppendQueryStringValue(url, arrKeyValue[0], arrKeyValue[1]);
                }
            }

            return url;
        }

        /// <summary>
        /// Adds the querystring value to an Url. Replace query string value if the querystring key exist.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="key">The querystring key.</param>
        /// <param name="value">The querystring value.</param>
        /// <param name="urlEncoded">if set to <c>true</c> [URL encoded].</param>
        /// <returns></returns>
        public static string AppendQueryStringValue(string url, string key, string value, bool urlEncoded = true)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            string[] arr = url.Split('?');

            if (arr.Length == 1)
                return string.Format("{0}?{1}={2}", url, key, urlEncoded ? UrlEncode(value) : value);
            else
            {
                var qs = arr[1];
                var query = new StringBuilder();
                var nvQS = HttpUtility.ParseQueryString(qs);
                bool isExistKey = false;

                foreach (string qskey in nvQS)
                {
                    if (string.Compare(qskey, key, true) == 0)
                    {
                        query.AppendFormat("{0}={1}&", qskey, urlEncoded ? UrlEncode(value) : value);
                        isExistKey = true;
                    }
                    else
                    {
                        query.AppendFormat("{0}={1}&", qskey, urlEncoded ? UrlEncode(nvQS[qskey]) : nvQS[qskey]);
                    }
                }

                if (!isExistKey)
                    query.AppendFormat("{0}={1}&", key, value);

                // remove last & from string
                query.Remove(query.Length - 1, 1);

                return string.Format("{0}?{1}", arr[0], query.ToString());
            }
        }

        #endregion

        #region File Type Thumbnail Icons

        private static Regex ExtensionRegex = new Regex("^.+\\.(?<extension>[^\\.]*)$");
        public static string GetExtension(string fileName)
        {
            Match m = ExtensionRegex.Match(fileName);
            if (m != null && m.Success)
                return m.Groups["extension"].Value;
            else
                return "";
        }

        #endregion

        #region String utilities

        /// <summary>
        /// Gets the alias format regular expression.
        /// </summary>
        /// <value>The alias format regular expression.</value>
        public static string AliasFormatRegularExpression
        {
            get
            {
                return "[a-zA-Z0-9][a-zA-Z0-9-_]+";
            }
        }

        /// <summary>
        /// Checks the alias is correct format for entire string.
        /// </summary>
        /// <param name="InputText">The input text.</param>
        /// <returns></returns>
        public static bool CheckAliasFormat(string InputText)
        {
            return CheckAliasFormat(InputText, true);
        }

        /// <summary>
        /// Checks the alias is correct format.
        /// </summary>
        /// <param name="InputText">The input text.</param>
        /// <param name="RequiredMatchEntiredString">if set to <c>true</c> validate on entire string, other, check on partial string.</param>
        /// <returns></returns>
        public static bool CheckAliasFormat(string InputText, bool RequiredMatchEntiredString)
        {
            bool IsValidFormat = false;
            string AliasRegEx = AliasFormatRegularExpression;

            // If you want to check the regular expression must match entired string, 
            // add \A at beginning and \Z at the end of expression
            // otherwise, the expression will return true if there is at least one partial match.
            if (RequiredMatchEntiredString)
                AliasRegEx = string.Format(@"\A{0}\Z", AliasRegEx);

            try
            {
                IsValidFormat = Regex.IsMatch(InputText, AliasRegEx);
            }
            catch (ArgumentException ex)
            {
                //LogHelper.Log4net(ex);
            }

            return IsValidFormat;
        }

        /// <summary>
        /// Generates the alias for input text.
        /// </summary>
        /// <param name="InputText">The input text.</param>
        /// <returns>Return the string that meets alias format. If input string contains only invalid characters, returns the empty string</returns>
        public static string GenerateAlias(string InputText)
        {
            string strAlias = string.Empty;

            var unicodeChars = "áéíóúñüÁÉÍÓÚÑæøåÆØÅåäöÅÄÖáàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            var replaceChars = "aeiounuAEIOUNaoaAOAaaoAAOaaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";

            try
            {
                int index = -1;

                while ((index = InputText.IndexOfAny(unicodeChars.ToCharArray())) != -1)
                {
                    int index2 = unicodeChars.IndexOf(InputText[index]);
                    InputText = InputText.Replace(InputText[index], replaceChars[index2]);
                }

                // Remove invalid character
                Regex ValidatorRegEx = new Regex(@"(^[^a-zA-Z0-9]+)|([^a-zA-Z0-9-_\s])");
                strAlias = ValidatorRegEx.Replace(InputText, " ");

                //Replace all space with _
                ValidatorRegEx = new Regex(@"\s+[-_]*");
                strAlias = ValidatorRegEx.Replace(strAlias, "-");
                strAlias = strAlias.Replace("--", "-");

                strAlias = strAlias.Trim(' ', '_', '-');
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
                // LogHelper.Log4net(new ClawException(ex));
            }

            return strAlias;
        }
        /// <summary>
        /// Generates the alias for input text.
        /// </summary>
        /// <param name="InputText">The input text.</param>
        /// <param name="ToLowerCase">Convert input text to lower case.</param>
        /// <returns>Return the string that meets alias format. If input string contains only invalid characters, returns the empty string</returns>
        public static string GenerateAlias(string InputText, bool ToLowerCase)
        {
            var result = GenerateAlias(InputText);
            return ToLowerCase ? result.ToLower() : result;
        }

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

        public static string GetApplicationPhysicalPath()
        {
            string AppPath = System.Web.HttpRuntime.AppDomainAppPath;
            if (!AppPath.EndsWith("/"))
                AppPath += "/";
            return AppPath;
        }

        public static string MapPath(string relativeFilePath)
        {
            if (!relativeFilePath.StartsWith("~/"))
                throw new ArgumentException("Globals.MapPath(string RelativeFilePath) supports only relative path, begins with ~/. Argument is: " + relativeFilePath);

            string result = GetApplicationPhysicalPath() + relativeFilePath.Substring(2);

            result = Regex.Replace(result, "(/)+", @"\");
            result = Regex.Replace(result, @"\\{2,}", Path.DirectorySeparatorChar.ToString());
            return result;
        }

        public static bool CheckWebFileExisted(string RelativeFilePath)
        {
            return System.IO.File.Exists(MapPath(RelativeFilePath));
        }

        /// <summary>
        /// Compares the date.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>Return 0 if x and y have the same date. Return 1 if X is larger (in the future) else, return -1</returns>
        public static int CompareDate(DateTime? x, DateTime? y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return 1;
            if (y == null)
                return -1;
            if (x.Value.Year > y.Value.Year)
                return 1;
            if (x.Value.Year < y.Value.Year)
                return -1;
            //same year
            if (x.Value.DayOfYear > y.Value.DayOfYear)
                return 1;
            if (x.Value.DayOfYear < y.Value.DayOfYear)
                return -1;
            return 0;
        }
        #endregion

        #region Cookie utilities

        private static string GetCookieName(string cookieName)
        {
            var ctx = HgtContext.Current;

            var name = "HGTCookie";
            name = string.Format("{0}-{1}", name, cookieName);

            return name;
        }

        /// <summary>
        /// Sets the cookie value to response
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="cookieValue">The cookie value.</param>
        /// <param name="days">Expired value in days.</param>
        /// <param name="hours">Expired value in hours.</param>
        /// <param name="minutes">Expired value minutes.</param>
        /// <param name="seconds">Expired value seconds.</param>
        public static void SetCookie(string cookieName, string cookieValue, int days, int hours, int minutes, int seconds)
        {
            var expiredDate = DateTime.Now.AddSeconds(seconds);
            expiredDate = expiredDate.AddMinutes(minutes);
            expiredDate = expiredDate.AddHours(hours);
            expiredDate = expiredDate.AddDays(days);

            var name = GetCookieName(cookieName);
            var cookie = new HttpCookie(name);
            cookie.Value = cookieValue;
            cookie.Expires = expiredDate;

            try
            {
                HgtContext.Current.Context.Response.Cookies.Add(cookie);
            }
            catch (Exception ex)
            {
                //LogHelper.Log4net(ex);
            }
        }

        /// <summary>
        /// Sets the cookie value to response
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="cookieValue">The cookie value.</param>
        /// <param name="days">Expired value in days.</param>
        public static void SetCookieInDays(string cookieName, string cookieValue, int days)
        {
            SetCookie(cookieName, cookieValue, days, 0, 0, 0);
        }

        /// <summary>
        /// Sets the cookie value to response
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="cookieValue">The cookie value.</param>
        /// <param name="hours">Expired value in hours.</param>
        public static void SetCookieInHours(string cookieName, string cookieValue, int hours)
        {
            SetCookie(cookieName, cookieValue, 0, hours, 0, 0);
        }

        /// <summary>
        /// Sets the cookie information minutes.
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="cookieValue">The cookie value.</param>
        /// <param name="minutes">Expired value minutes.</param>
        public static void SetCookieInMinutes(string cookieName, string cookieValue, int minutes)
        {
            SetCookie(cookieName, cookieValue, 0, 0, minutes, 0);
        }

        /// <summary>
        /// Sets the cookie value to response
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="cookieValue">The cookie value.</param>
        /// <param name="seconds">>Expired value seconds.</param>
        public static void SetCookieInSeconds(string cookieName, string cookieValue, int seconds)
        {
            SetCookie(cookieName, cookieValue, 0, 0, 0, seconds);
        }

        /// <summary>
        /// Sets the cookie value to response to default expired value from site settings
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="cookieValue">The cookie value.</param>
        public static void SetCookie(string cookieName, string cookieValue)
        {
            int timeout = 0;

            if (timeout > 0)
                SetCookie(cookieName, cookieValue, 0, 0, 0, timeout);
            else
                SetCookie(cookieName, cookieValue, 30, 0, 0, 0);
        }

        /// <summary>
        /// Gets the cookie from current request.
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <returns></returns>
        public static string GetCookie(string cookieName)
        {
            string name = GetCookieName(cookieName);

            HttpCookie cookie = HgtContext.Current.Context.Request.Cookies[name];
            if (cookie != null)
                return cookie.Value;

            return string.Empty;
        }

        /// <summary>
        /// Removes the cookie from request object
        /// </summary>
        /// <param name="cookieName">Name of the cookie.</param>
        public static void RemoveCookie(string cookieName)
        {
            string name = GetCookieName(cookieName);

            HttpCookie cookie = HgtContext.Current.Context.Request.Cookies[name];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d);
                HgtContext.Current.Context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Removes all cookie match the pattern from request object
        /// </summary>
        /// <param name="cookiePattern">The cookie pattern.</param>
        public static void RemoveCookieByPattern(string cookiePattern)
        {
            HttpContext ctx = HgtContext.Current.Context;
            string[] keys = ctx.Request.Cookies.Keys.Cast<string>().ToArray<string>();

            Regex regEx = new Regex(cookiePattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (string key in keys)
            {
                if (regEx.IsMatch(key))
                    RemoveCookie(key);
            }
        }

        /// <summary>
        /// Removes all cookie.
        /// </summary>
        public static void RemoveAllCookie()
        {
            HttpContext ctx = HgtContext.Current.Context;
            string[] keys = ctx.Request.Cookies.Keys.Cast<string>().ToArray<string>();

            foreach (string cookie in keys)
            {
                RemoveCookie(cookie);
            }
        }

        #endregion

        #region copy/delete files/foder

        /// <summary>
        /// Copies the folder.
        /// </summary>
        /// <param name="SourceFolder">The source folder.</param>
        /// <param name="DestinationFolder">The destination folder.</param>
        public static void CopyFolder(string SourceFolder, string DestinationFolder)
        {
            SourceFolder = SourceFolder.TrimEnd('\\');
            DestinationFolder = DestinationFolder.TrimEnd('\\');

            if (!System.IO.Directory.Exists(DestinationFolder))
                System.IO.Directory.CreateDirectory(DestinationFolder);

            System.IO.DirectoryInfo oSource = new System.IO.DirectoryInfo(SourceFolder);
            foreach (System.IO.DirectoryInfo oDirectory in oSource.GetDirectories())
            {
                System.IO.Directory.CreateDirectory(DestinationFolder + "\\" + oDirectory.Name);
                CopyFolder(oDirectory.FullName, DestinationFolder + "\\" + oDirectory.Name);
            }

            foreach (System.IO.FileInfo oFile in oSource.GetFiles())
                System.IO.File.Copy(oFile.FullName, DestinationFolder + "\\" + oFile.Name, true);
        }

        public static void DeleteFolder(string path)
        {
            Thread deleteThread = new Thread(DeleteFolderThread);
            deleteThread.Start(path);
        }

        public static void DeleteFilesInFolder(string path)
        {
            System.IO.DirectoryInfo oSource = new System.IO.DirectoryInfo(path);
            DirectoryInfo[] subDirs = oSource.GetDirectories();
            FileInfo[] innerFiles = oSource.GetFiles();

            foreach (FileInfo fi in innerFiles)
            {
                fi.Delete();
            }

            foreach (System.IO.DirectoryInfo oDirectory in subDirs)
            {
                DeleteFilesInFolder(oDirectory.FullName);
            }
            oSource.Parent.Refresh();
        }

        private static void DeleteFolderThread(object FolderName)
        {
            string sFname = FolderName as string;
            if (string.IsNullOrEmpty(sFname))
                return;
            if (Directory.Exists(sFname))
                Directory.Delete(sFname, true);
        }

        #endregion

        #region image path, file path

        public static string AbsoluteImageDefault
        {
            get
            {
                var fileDefault = ConfigurationManager.AppSettings[ConstantKeys.ImageDefault];
                return ResolveClientUrl(ImagePath(fileDefault));
            }
        }

        public static string TempFolder
        {
            get
            {
                var tmp = ConfigurationManager.AppSettings[ConstantKeys.TempFolder];
                tmp = tmp.TrimEnd('/');
                return tmp;
            }
        }

        public static string TempThumbFolder
        {
            get
            {
                var tmp = ConfigurationManager.AppSettings[ConstantKeys.TempThumbFolder];
                tmp = tmp.TrimEnd('/');
                return tmp;
            }
        }

        public static string ThumbFolder
        {
            get
            {
                var tmp = ConfigurationManager.AppSettings[ConstantKeys.ThumbFolder];
                tmp = tmp.TrimEnd('/');
                return tmp;
            }
        }

        public static string ImageFolder
        {
            get
            {
                var tmp = ConfigurationManager.AppSettings[ConstantKeys.ImageFolder];
                tmp = tmp.TrimEnd('/');
                return tmp;
            }
        }

        public static string ImageSubFolder(string folder)
        {
            var patch = ImageFolder;
            if (!string.IsNullOrEmpty(folder))
            {
                if (!patch.EndsWith("/"))
                    patch = patch + "/";

                patch = patch + folder;
            }
            return patch;
        }


        public static string TempImagePath(string fileName)
        {
            var patch = TempFolder;
            if (!patch.EndsWith("/"))
                patch = patch + "/";
            return VirtualPathUtility.Combine(patch, fileName);
        }

        public static string AbsoluteTempImagePath(string fileName)
        {
            return Globals.ResolveClientUrl(TempImagePath(fileName));
        }

        public static string ImagePath(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var patch = ImageFolder;
                if (!patch.EndsWith("/"))
                    patch = patch + "/";
                return VirtualPathUtility.Combine(patch, fileName);
            }
            return "";
        }

        public static string AbsoluteImagePath(string fileName)
        {
            return Globals.ResolveClientUrl(ImagePath(fileName));
        }

        public static string ImagePath(string folder, string fileName = "")
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var patch = ImageSubFolder(folder);

                if (!string.IsNullOrEmpty(fileName))
                {
                    if (!patch.EndsWith("/"))
                        patch = patch + "/";
                    return VirtualPathUtility.Combine(patch, fileName);
                }
                return patch;
            }
            return "";
        }

        public static string AbsoluteImagePath(string folder, string fileName = "")
        {
            return Globals.ResolveClientUrl(ImagePath(folder, fileName));
        }

        public static string ThumbPath(string folder, string fileName = "")
        {
            var patch = ImageSubFolder(folder);

            if (!patch.EndsWith("/"))
                patch = patch + "/";

            patch = patch + ThumbFolder;

            if (!string.IsNullOrEmpty(fileName))
            {
                if (!patch.EndsWith("/"))
                    patch = patch + "/";
                return VirtualPathUtility.Combine(patch, fileName);
            }

            return patch;
        }

        public static string AbsoluteThumbPath(string folder, string filename = "")
        {
            return Globals.ResolveClientUrl(ThumbPath(folder, filename));
        }

        #endregion
    }
}
