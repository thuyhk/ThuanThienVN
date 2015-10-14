using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Gallery : Entity
    {
        public Gallery()
        {
            this.GalleryDetails = new List<GalleryDetail>();
        }

        public int Id  { get; set; }
        public int ObjectId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int Type { get; set; }
        public int Category { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<GalleryDetail> GalleryDetails { get; set; }
    }
}
