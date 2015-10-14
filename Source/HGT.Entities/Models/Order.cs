using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Order : Entity
    {
        public int Id { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int OrderStatusId { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string Address { get; set; }
        public Nullable<int> StateProvinceId { get; set; }
        public Nullable<int> DistrictId { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Note { get; set; }
        public Nullable<bool> HomeDelivery { get; set; }
    }
}
