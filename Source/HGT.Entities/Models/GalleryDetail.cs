using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class GalleryDetail : Entity
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string Thumbnail { get; set; }
        public bool IsVideo { get; set; }
        public int DisplayOrder { get; set; }

        public int GalleryId { get; set; }
        public virtual Gallery Gallery { get; set; }
    }
}
