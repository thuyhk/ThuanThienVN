using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HGT.Entities.Models;

namespace HGT.Web.Models
{
    public class CheckoutModel
    {
        [DisplayName("Tên khách hàng")]
        [Required(ErrorMessage = "* Nhập tên khách hàng")]
        public string FullName { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "* Nhập số điện thoại liên hệ")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [RegularExpression("[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?", ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [DisplayName("Địa chỉ giao hàng")]
        [Required(ErrorMessage = "* Nhập địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        public bool HomeDelivery { get; set; }

        [DisplayName("Ghi chú")]
        public string Note { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}