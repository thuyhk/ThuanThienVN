using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HGT.Core.Models
{
    public class UserModel 
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password ")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "* Confirm password not correct")]
        public string ConfirmPassword { get; set; }

        public string PasswordSalt { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Full name")]
        public string FullName { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "* Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email format not correct")]
        public string Email { get; set; }

        public string Company { get; set; }

        public string Phone { get; set; }
        public string Mobile { get; set; }

        [DisplayName("Avatar")]
        public string Image { get; set; }

        public bool IsAdmin { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [DisplayName("Role name")]
        [Required(ErrorMessage = "* Select role name")]
        public int RoleId { get; set; }

        public DateTime LastLogonDate { get; set; }
        public bool Gender { get; set; }
        public string TokenForgotPassWord { get; set; }

        //
        public string RoleName { get; set; }
        public bool ShowDelete { get; set; }
        public bool showUpdate { get; set; }


        public IList<SelectListItem> RoleList { get; set; }
        public UserModel()
        {
            RoleList = new List<SelectListItem>();
        }
    }
}
