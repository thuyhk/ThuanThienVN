using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace HGT.Core.Models
{
    public partial class SolutionModel : BaseModel
    {
        public SolutionModel()
        {
            this.Products = new List<Product>();
            this.Galleries = new List<GalleryDetail>();
        }

        #region Properties

        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập tiêu đề giải pháp")]
        [Display(Name = "Tiêu đề")]
        public string Name { get; set; }

        [Display(Name = "Alias")]
        [Remote("ValidateAlias", "Solution", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Tiêu đề đã tồn tại. Vui lòng chọn tiêu đề khác!")]
        [Required(ErrorMessage = "Nhập Alias")]
        public string Alias { get; set; }

        [Display(Name = "Tags")]
        public string MetaKey { get; set; }

        [Display(Name = "Danh mục cha")]
        public Nullable<int> RootId { get; set; }

        [Display(Name = "Thứ tự sắp xếp")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Thumbnail { get; set; }

        [Display(Name = "Hiển thị")]
        public bool IsActive { get; set; }

        [Display(Name = "Nổi bật")]
        public bool IsFeature { get; set; }

        [Display(Name = "Mô tả ngắn")]
        [AllowHtml]
        public string Brief { get; set; }

        [Display(Name = "Chi tiết")]
        [AllowHtml]
        public string Description { get; set; }        

        [Display(Name = "Hiển thị thư viện")]
        public bool EnableGallery { get; set; }

        [Display(Name = "Mở trang mới khi click")]
        public bool OpenLink { get; set; }

        [Display(Name = "Link đến trang mới")]
        public string Link { get; set; }

        #endregion

        #region Other properties

        public virtual List<Product> Products { get; set; }
        public virtual List<GalleryDetail> Galleries { get; set; }

        private string _thumbPath;
        public virtual string ThumbPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.Thumbnail))
                    _thumbPath = Globals.AbsoluteImageDefault;
                else
                {
                    var url = Globals.AbsoluteThumbPath(ConstantKeys.SolutionFolder, this.Thumbnail);
                    _thumbPath = url;
                }
                return _thumbPath;
            }
            set { _thumbPath = value; }
        }

        public virtual int TotalGallery { get; set; }         

        #endregion

    }
}
