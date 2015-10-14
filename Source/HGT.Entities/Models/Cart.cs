using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Cart : Entity
    {
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }

        public virtual Product Product { get; set; }

        [NotMapped]
        public decimal Total
        {
            get
            {
                _total = Count * Product.Price;
                return _total;
            }
            set { _total = value; }
        }

        [NotMapped]
        public string CategoryAlias { get; set; }

        private decimal _total;
    }
}
