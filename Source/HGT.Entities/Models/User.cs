using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class User : Entity
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
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int RoleId { get; set; }
        public Nullable<System.DateTime> LastLogonDate { get; set; }
        public bool Gender { get; set; }
        public string TokenForgotPassWord { get; set; }
    }
}
