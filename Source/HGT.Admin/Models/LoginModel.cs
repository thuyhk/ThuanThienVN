using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HGT.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool CaptchaShow { get; set; }

        [Required(ErrorMessage = "* Required")]
        public string Captcha { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string Error { get; set; }
    }
}