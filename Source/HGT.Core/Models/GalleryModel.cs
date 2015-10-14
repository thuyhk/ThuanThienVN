using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HGT.Entities.Models;
using System.ComponentModel.DataAnnotations;

namespace HGT.Core.Models
{
    public partial class GalleryModel : BaseModel
    {
        [Display(Name = "Tên hình ảnh")]
        public string FileName { get; set; }

        [Display(Name = "Link liên kết")]
        public string Url { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Thumbnail { get; set; }

        [Display(Name = "Đây là video")]
        public bool IsVideo { get; set; }

        [Display(Name = "Thứ tự")]
        public int DisplayOrder { get; set; }

        public int GalleryId { get; set; }
        public virtual Gallery Gallery { get; set; }

        private string _thumbPath;
        public string ThumbPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.Thumbnail))
                    _thumbPath = Globals.AbsoluteImageDefault;
                else
                {
                    var url = Globals.AbsoluteThumbPath(ConstantKeys.GalleryFolder, this.Thumbnail);
                    _thumbPath = url;
                }
                return _thumbPath;
            }
            set { _thumbPath = value; }
        }
    }
}
