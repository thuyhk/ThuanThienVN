using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace HGT.Entities.Models
{
    public partial class SolutionProductMapping : Entity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SolutionId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
