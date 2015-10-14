using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HGT.Core.Models
{
    public class ContentModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Name { get; set; }

        public string Alias { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Content")]
        public string Value { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
