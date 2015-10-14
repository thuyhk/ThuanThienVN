using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Category : Entity
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string MetaKey { get; set; }
        public Nullable<int> RootId { get; set; }
        public int DisplayOrder { get; set; }
        public string Thumbnail { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsFeature { get; set; }
        public string Brief { get; set; }
        public string Description { get; set; }
        public string HowIt { get; set; }
        public string KeyBenefit { get; set; }
        public string Solution { get; set; }
        public bool EnableGallery { get; set; }
        public bool EnablePrintCapbility { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
