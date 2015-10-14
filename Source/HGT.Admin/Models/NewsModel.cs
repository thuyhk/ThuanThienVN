using HGT.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HGT.Admin.Models
{
    public class NewsModel
    {
        public int Id { get; set; }

         [Display(Name = "Title")]
        public string Name { get; set; }

         [Display(Name = "Alias")]
        public string Alias { get; set; }

         [Display(Name = "Meta key")]
        public string MetaKey { get; set; }

        public string Description { get; set; }

        [AllowHtml]
        public string Content { get; set; }
        public Nullable<int> CountView { get; set; }
        public Nullable<bool> ShowHomePage { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Picture { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        private string _thumbnail;
        public string Thumbnail
        {
            get
            {
                if (string.IsNullOrEmpty(this.Picture))
                    _thumbnail = Globals.AbsoluteImageDefault;
                else
                {
                    var url = Globals.ThumbPath(ConstantKeys.NewsFolder, this.Picture);
                    _thumbnail = url;
                }
                return _thumbnail;
            }
            set { _thumbnail = value; }
        }
    }
}
