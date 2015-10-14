using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HGT.Web.Models
{
    public class ContactModel
    {
        public ContactModel()
        {
            this.Departments = new List<SelectListItem>();
        }

        [Required(ErrorMessage = "Nhập tên liên hệ")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Nhập email liên hệ")]
        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Email không hợp lệ.")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Nhập số điện thoại")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Nhập nội dung")]
        public string ContactContent { get; set; }

        public string CompanyName { get; set; }

        public int DepartmentId { get; set; }
        public virtual IList<SelectListItem> Departments { get; set; }

        public bool CaptchaShow { get; set; }

        [Required(ErrorMessage = "Nhập mã xác thực")]
        public string Captcha { get; set; }

    }
}