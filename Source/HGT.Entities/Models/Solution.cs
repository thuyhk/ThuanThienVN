using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Solution : Entity
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string MetaKey { get; set; }
        public Nullable<int> RootId { get; set; }
        public int DisplayOrder { get; set; }
        public string Thumbnail { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsFeature { get; set; }
        public string Brief { get; set; }
        public string Description { get; set; }
        public bool EnableGallery { get; set; }
        public bool OpenLink { get; set; }
        public string Link { get; set; }
    }
}
