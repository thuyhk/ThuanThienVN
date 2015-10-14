using HGT.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HGT.Admin.Controllers
{
    public class RoleController : AdminController
    {
        /// <summary>
        /// View of access deny
        /// </summary>
        /// <returns></returns>
        public ActionResult AccessDeny()
        {
            return View();
        }
    }
}
