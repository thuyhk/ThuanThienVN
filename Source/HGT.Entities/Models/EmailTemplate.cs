using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class EmailTemplate : Entity
    {
        public int EmailTemplateId { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool Deleteable { get; set; }
        public System.DateTime LastModified { get; set; }
    }
}
