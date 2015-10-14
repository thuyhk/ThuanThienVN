using HGT.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Core.Models
{
    public class BannerModel : BaseModel
    {
        [DisplayName("Tiêu đề")]
        [Required(ErrorMessage="Nhập tiêu đề")]
        public string Title { get; set; }

         [DisplayName("Mô tả ngắn")]
        public string Description { get; set; }

        [DisplayName("Link liên jeets")]
        public string Link { get; set; }

        [DisplayName("Mở link tab mới")]
        public bool OpenLink { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Hình ảnh")]
        public string FileName { get; set; }

        private string _imagePath;
        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.FileName))
                    _imagePath = Globals.AbsoluteImageDefault;
                else
                {
                    var url = Globals.AbsoluteImagePath(ConstantKeys.BannerFolder, this.FileName);
                    _imagePath = url;
                }
                return _imagePath;
            }
            set { _imagePath = value; }
        }

        [DisplayName("Thứ tự sắp xếp")]
        public int DisplayOrder { get; set; }

        [DisplayName("Hiển thị")]
        public bool IsActive { get; set; }
    }
}
