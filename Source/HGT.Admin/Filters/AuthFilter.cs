using HGT.Admin.Models;
using HGT.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HGT.Admin.Filters
{

    public class AuthFilter : AuthorizeAttribute
    {
        bool isAuthorize = false;
        public AuthFilter() { }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAllow = true;
            isAuthorize = false;

            //Check user has logged in or not
            var usIn = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;

            string url = httpContext.Request.RawUrl;

            if (usIn != null)
            {
                isAuthorize = true;
            }
            else
            {
                return false;
            }

            if (url.ToLower() == "/admin/Role/AccessDeny")
                return true;

            //Check user has permission or not
            if (usIn.MaskPermission <= 0)
                return false;

            var bitMask = usIn.MaskPermission;
            //Check permission of user
            foreach (string item in Permissions.listUrlDenyByUser)
            {
                string[] arr = item.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (Convert.ToInt32(arr[2]) >= bitMask)
                {
                    //With short URL ex:/admin/user
                    if (arr[1] == "0")
                    {
                        if (url.ToLower().Contains(arr[0].ToLower()))
                        {
                            isAllow = false;
                            break;
                        }
                    }
                }
                //else if (url.ToLower().Contains(arr[0].ToLower()))
                //{
                //    isAllow = false;
                //    break;
                //}
            }

            return isAllow;

        }
        /// <summary>
        /// Method to process when authorization is fail
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //If user has not logged in will redirect to login page
            if (!isAuthorize)
            {
                var Url = HttpContext.Current.Request.Url.AbsolutePath;
                filterContext.Result = new RedirectResult("/common/?url=" + Url);
            }
            else//if user has not enough permission will redirect to access deny page
            {
                filterContext.Result = new RedirectResult("/admin/Role/AccessDeny");
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

    }
}
