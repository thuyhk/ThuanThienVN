using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class News : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string MetaKey { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsHighlight { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
