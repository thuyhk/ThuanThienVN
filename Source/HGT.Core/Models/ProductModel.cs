using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HGT.Core.Models
{
    public partial class ProductModel : BaseModel
    {
        #region Properties
        [DisplayName("Mã code")]
        public string SKU { get; set; }

        [DisplayName("Tên")]
        [Required(ErrorMessage = "Nhập tên sản phẩm")]
        public string Name { get; set; }

        [DisplayName("Danh mục")]
        [Required(ErrorMessage = "Chọn danh mục")]
        public int CategoryId { get; set; }

        [Remote("ValidateAlias", "Product", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "The value is duplicate!")]
        [DisplayName("Alias")]
        [Required(ErrorMessage = "Nhập alias")]
        public string Alias { get; set; }

        [DisplayName("Mô tả ngắn")]
        public string Brief { get; set; }

        [AllowHtml]
        [DisplayName("Tính năng")]
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string Unit { get; set; }

        [DisplayName("Hiển thị")]
        public bool IsActive { get; set; }
        public string Origin { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Thumbnail { get; set; }
        public bool CallPrice { get; set; }

        [AllowHtml]       
        public string KeyBenefit { get; set; }

        [AllowHtml]
        [DisplayName("Thông số kỹ thuật")]
        public string Capability { get; set; }

        [AllowHtml]
        public string CaseStudy { get; set; }

        [DisplayName("Hiển thị thư viện")]
        public bool EnableGallery { get; set; }

        [DisplayName("Thứ tự sắp xếp")]
        public int DisplayOrder { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        [DisplayName("File attachment")]
        public string FileAttach { get; set; }

        public string FileAttachName { get; set; }

        #endregion

        #region Other properties

        public virtual Category Category { get; set; }
        public virtual List<GalleryDetail> Galleries { get; set; }
        public List<GalleryDetail> Prints { get; set; }

        private string _thumbPath;
        public string ThumbPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.Thumbnail))
                    _thumbPath = Globals.AbsoluteImageDefault;
                else
                {
                    var url = Globals.AbsoluteThumbPath(ConstantKeys.ProductFolder, this.Thumbnail);
                    _thumbPath = url;
                }
                return _thumbPath;
            }
            set { _thumbPath = value; }
        }

        public int TotalGallery { get; set; }
        public int TotalPrint { get; set; }

        public string CategoryAlias { get; set; }


        public string FileAttachTemp { get; set; }

        #endregion
    }
}
