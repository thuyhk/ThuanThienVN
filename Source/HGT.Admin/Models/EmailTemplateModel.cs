using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HGT.Admin.Models
{
    public class EmailTemplateModel
    {
        public int EmailTemplateId { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Subject")]
        public string Subject { get; set; }

        [DisplayName("Content")]
        [AllowHtml]
        public string Content { get; set; }
        public bool Deleteable { get; set; }
        public DateTime LastModified { get; set; }
    }
}
