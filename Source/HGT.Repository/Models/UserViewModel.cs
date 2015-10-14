using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Repository.Models;

namespace HGT.Repository.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Image { get; set; }
        public bool IsAdmin { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int RoleId { get; set; }
        public DateTime LastLogonDate { get; set; }
        public bool Gender { get; set; }
        public string TokenForgotPassWord { get; set; }

        //
        public string RoleName { get; set; }
        public bool ShowDelete { get; set; }
        public bool showUpdate { get; set; }

    }

}
