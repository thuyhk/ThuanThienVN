using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;

namespace HGT.Core.Models
{
    public class OrderDetailModel
    {
        public int OderDetailId { get; set; }
        public int OrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }


    }
}
