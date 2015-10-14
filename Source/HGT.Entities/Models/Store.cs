using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Store : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string OpenTime { get; set; }
        public string Picture { get; set; }
        public string Content { get; set; }
        public string Alias { get; set; }
    }
}
