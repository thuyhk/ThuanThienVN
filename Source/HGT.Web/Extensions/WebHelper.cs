using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HGT.Core.Enums;
using HGT.Entities.Models;
using HGT.Web.Models;
using HGT.Core;
using HGT.Core.Extensions;

namespace HGT.Web.Extensions
{
    public class WebHelper
    {
        public WebHelper()
        {
        }

        public string GetContactPage()
        {
            using (var dtx = new HGTContext())
            {
                var result = dtx.Contents.Find((int)ContentType.Contact).Value;
                return result;
            }
        }

        public string GetContactOss()
        {
            using (var dtx = new HGTContext())
            {
                var result = dtx.Contents.Find((int)ContentType.Footer).Value;
                return result;
            }
        }        
    }
}