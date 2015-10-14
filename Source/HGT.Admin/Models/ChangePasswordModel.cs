using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Admin.Models
{
    public class ChangePasswordModel
    {
        public string EmailCF { get; set; }
        [Required (ErrorMessage ="* Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "* Confirm password not correct")]
        public string ConfirmPassword { get; set; }

        public string TokenPassword { get; set; }
    }
}
