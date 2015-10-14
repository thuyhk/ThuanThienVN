using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Content : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Value { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
    }
}
