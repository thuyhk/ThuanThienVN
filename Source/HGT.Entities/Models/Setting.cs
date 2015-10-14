using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class Setting : Entity
    {
        public int Id { get; set; }
        public string Values { get; set; }
        public string ContactPage { get; set; }
        public string ContactOss { get; set; }
        public string IntroPage { get; set; }
    }
}
