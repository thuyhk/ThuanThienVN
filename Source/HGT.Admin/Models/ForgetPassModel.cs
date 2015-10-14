using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Admin.Models
{
    public class ForgetPassModel
    {
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email format not correct")]
        public string EmailTo { get; set; }

        public bool CaptchaShow { get; set; }
        [Required(ErrorMessage = "* Required")]
        public string Captcha { get; set; }

    }
}
