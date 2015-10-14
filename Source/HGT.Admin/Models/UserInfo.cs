using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HGT.Admin.Models
{
    public class UserInfo
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public bool? Active { get; set; }
        public List<int> BitMask { get; set; }
        public int MaskPermission { get; set; }

        public bool IsSA { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; }
        public bool IsUser { get; set; }
    }
}