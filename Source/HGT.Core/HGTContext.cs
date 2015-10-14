using HGT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HGT.Core
{
    public sealed class HgtContext
    {
        static readonly string dataKey = "HGTContextStore";

        private HgtContext()
        { }

        private HgtContext(HttpContext context)
        {
            if (context != null)
            {
                Context = context;
                if (context.Request != null)
                {
                    QueryString = context.Request.QueryString;
                    RawUrl = context.Request.RawUrl;
                }

                //DataService = IoC.Kernel.Get<IDataService>();
            }
        }

        /// <summary>
        /// Returns the current instance of the ClawContext from the ThreadData Slot. If one is not found and a valid HttpContext can be found,
        /// it will be used. Otherwise, an exception will be thrown. 
        /// </summary>
        public static HgtContext Current
        {
            get
            {
                HttpContext httpContext = HttpContext.Current;

                if (httpContext != null)
                {
                    if (httpContext.Items.Contains(dataKey))
                        return httpContext.Items[dataKey] as HgtContext;
                    else
                    {
                        HgtContext context = new HgtContext(httpContext);
                        if (context.Context != null)
                            context.Context.Items[dataKey] = context;

                        return context;
                    }
                }

                return new HgtContext();
            }
        }

        public NameValueCollection QueryString
        {
            get;
            set;
        }

        public string RawUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current site URL from request
        /// </summary>
        /// <value>
        /// The site URL.
        /// </value>
        public string SiteUrl
        {
            get
            {
                string siteUrl = string.Empty;

                if (Context != null)
                {
                    siteUrl = string.Format("{0}://{1}", Context.Request.Url.Scheme, SiteDomain);
                    if (Context.Request.Url.Port != 80)
                        siteUrl = string.Format("{0}://{1}:{2}", Context.Request.Url.Scheme, SiteDomain, Context.Request.Url.Port);
                }

                return siteUrl;
            }
        }

        /// <summary>
        /// Gets the current site domain from request
        /// </summary>
        /// <value>
        /// The site domain.
        /// </value>
        public string SiteDomain
        {
            get
            {
                string siteDomain = string.Empty;

                if (Context != null)
                {
                    string hostName = Context.Request.Url.Host.Replace("www.", string.Empty);
                    string applicationPath = Context.Request.ApplicationPath;

                    if (applicationPath.EndsWith("/"))
                        applicationPath = applicationPath.Remove(applicationPath.Length - 1, 1);

                    siteDomain = string.Format("{0}{1}", hostName, applicationPath);
                }

                return siteDomain;
            }
        }

        public HttpContext Context
        {
            get;
            private set;
        }


        public bool IsWebRequest
        {
            get { return Context != null; }
        }

        public bool IsAjaxRequest
        {
            get
            {
                return Context != null &&
                       string.Equals("XMLHttpRequest", Context.Request.Headers["x-requested-with"],
                           StringComparison.OrdinalIgnoreCase);
            }
        }

        public RouteData RouteData
        {
            get
            {
                return Context.Request.RequestContext.RouteData;
            }
        }

        public string UserIp
        {
            get
            {
                return Context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }

        public string ServerIp
        {
            get
            {
                var localIps = Dns.GetHostAddresses(Dns.GetHostName());
                var ip = localIps != null ? localIps.FirstOrDefault(a => a.GetAddressBytes().Length == 4) : null;

                return ip != null ? ip.ToString() : "::1";
            }
        }

        public HttpBrowserCapabilities HttpBrowserCapabilities
        {
            get { return Context.Request.Browser; }
        }

        public string BrowserDetectionInfo
        {
            get
            {
                var browserDetectionInfo = string.Empty;

                if (Current.HttpBrowserCapabilities == null)
                {
                    return "js";
                }

                var userAgent = Current.HttpBrowserCapabilities.Browser.ToLower();
                var platform = Current.HttpBrowserCapabilities.Platform.ToLower();
                var version = Current.HttpBrowserCapabilities.Version.ToLower();
                var mobileDevice = Current.HttpBrowserCapabilities.MobileDeviceModel.ToLower();

                if (Regex.IsMatch(userAgent, @"ie") || Regex.IsMatch(userAgent, @"internetexplorer"))
                {
                    browserDetectionInfo = "ie ie" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                }
                else if (Regex.IsMatch(userAgent, @"firefox"))
                {
                    browserDetectionInfo = "ff ff" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                }
                else if (Regex.IsMatch(userAgent, @"gecko"))
                {
                    browserDetectionInfo = "gecko";
                }
                else if (Regex.IsMatch(userAgent, @"opera"))
                {
                    browserDetectionInfo = "opera opera" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                }
                else if (Regex.IsMatch(userAgent, @"konqueror"))
                {
                    browserDetectionInfo = "konqueror";
                }
                else if (Regex.IsMatch(userAgent, @"chrome"))
                {
                    browserDetectionInfo = "chrome chrome" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                }
                else if (Regex.IsMatch(userAgent, @"safari"))
                {
                    if (Regex.IsMatch(platform, @"unknown"))
                    {
                        browserDetectionInfo = "safari";
                    }
                    else
                    {
                        browserDetectionInfo = "safari safari" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                    }

                }
                else if (Regex.IsMatch(userAgent, @"mozilla"))
                {
                    browserDetectionInfo = "gecko";
                    if (Regex.IsMatch(Current.HttpBrowserCapabilities.Id, @"webkit"))
                    {
                        browserDetectionInfo = "safari safari" + (!string.IsNullOrEmpty(version) && Regex.IsMatch(version, @"\d+") ? version.Split(new[] { '.', ',' }).FirstOrDefault() : string.Empty);
                    }
                }

                if (Regex.IsMatch(platform, @"j2me"))
                {
                    browserDetectionInfo += " mobile";
                }
                else if (Regex.IsMatch(platform, @"darwin"))
                {
                    browserDetectionInfo += " mac";
                }
                else if (Regex.IsMatch(platform, @"x11"))
                {
                    browserDetectionInfo += " linux";
                }
                else if (Regex.IsMatch(platform, @"winnt"))
                {
                    browserDetectionInfo += " win";
                }
                else if (Regex.IsMatch(platform, @"unknown"))
                {
                    browserDetectionInfo += string.Format(" {0}", mobileDevice);
                }

                else
                {
                    browserDetectionInfo += " " + platform.ToLower();
                }

                browserDetectionInfo += " js";
                return browserDetectionInfo;
            }
        }

        ///// <summary>
        ///// Gets default return URL query key.
        ///// </summary>
        ///// <value>
        ///// Default return URL query key.
        ///// </value>
        //public string ReturnUrlQueryKey
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["aspnet:FormsAuthReturnUrlVar"] ?? "returnUrl";
        //    }
        //}

        public string GetCurrentSiteURL()
        {
            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null))
            {
                System.Collections.Specialized.NameValueCollection variables = HttpContext.Current.Request.ServerVariables;
                string protocol = (variables["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
                string domain = variables["SERVER_NAME"];
                string port = variables["SERVER_PORT"];
                if (port == "80")
                {
                    port = String.Empty;
                }
                else
                {
                    port = ":" + port;
                }
                // 2nd
                return (protocol + domain + port);
            }
            else
            {
                return string.Empty;
            }
        }
       
    }
}
