using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Banner : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool OpenLink { get; set; }
        public string FileName { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
