using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Product : Entity
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Alias { get; set; }
        public string Brief { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }       
        public Nullable<bool> IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string Origin { get; set; }
        public string Thumbnail { get; set; }
        public Nullable<bool> CallPrice { get; set; }
        public string KeyBenefit { get; set; }
        public string Capability { get; set; }
        public string CaseStudy { get; set; }
        public bool EnableGallery { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Category Category { get; set; }

        public string FileAttach { get; set; }

        public string FileAttachName { get; set; }
    }
}
