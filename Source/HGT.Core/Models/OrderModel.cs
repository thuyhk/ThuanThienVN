using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Core.Enums;
using HGT.Entities.Models;

namespace HGT.Core.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public Nullable<int> CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int OrderStatusId { get; set; }

        public Nullable<decimal> Total { get; set; }

        public string Address { get; set; }

        public int StateProvinceId { get; set; }

        public Nullable<int> DistrictId { get; set; }

        public Nullable<bool> Deleted { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public string Note { get; set; }

        public Nullable<bool> HomeDelivery { get; set; }       

        private string _orderStatus { get; set; }
        public string OrderStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(_orderStatus))
                    return _orderStatus;
                if (this.OrderStatusId == 1)
                    _orderStatus = OrderStatusType.New.ToString();
                else if (this.OrderStatusId == 2)
                    _orderStatus = OrderStatusType.Process.ToString();
                else if (this.OrderStatusId == 3)
                    _orderStatus = OrderStatusType.Completed.ToString();
                else if (this.OrderStatusId == 4)
                    _orderStatus = OrderStatusType.Cancel.ToString();
                else
                    _orderStatus = "None";
                return _orderStatus;

            }
            set { _orderStatus = value; }
        }

        public List<Product> Products { get; set; }
        
    }
}
