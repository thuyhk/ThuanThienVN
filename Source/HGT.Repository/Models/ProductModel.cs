using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;

namespace HGT.Repository.Models
{
    public class ProductModel
    {
        public ProductModel()
        {            
            Pictures = new List<ProductPicture>();
        }

        #region Properties

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SeoName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool ShowIndex { get; set; }
        public string Origin { get; set; }
        public string Picture { get; set; }
        public bool CallPrice { get; set; }

        #endregion

        #region Other properties

        public Category Category { get; set; }
        public IList<ProductPicture> Pictures { get; set; } // list pictures        

        #endregion

    }
}
