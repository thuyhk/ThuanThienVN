using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Status : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Type { get; set; }
    }
}
