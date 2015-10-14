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
    public partial class CategoryModel : BaseModel
    {
        public CategoryModel()
        {
            this.Products = new List<Product>();
            this.Galleries = new List<GalleryDetail>();
            this.Prints = new List<GalleryDetail>();
        }

        #region Properties

        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập dữ liệu!")]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [Display(Name = "Alias")]
        [Remote("ValidateAlias", "Category", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "Danh mục đã tồn tại. Vui lòng chọn tên khác!")]
        [Required(ErrorMessage = "Vui lòng nhập dữ liệu!")]
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

        [Display(Name = "Tổng quan")]
        [AllowHtml]
        public string Brief { get; set; }

        [Display(Name = "Nội dung chi tiết")]
        [AllowHtml]
        public string Description { get; set; }

        [Display(Name = "How it works")]
        [AllowHtml]
        public string HowIt { get; set; }

        [Display(Name = "Key benefit")]
        [AllowHtml]
        public string KeyBenefit { get; set; }

        [Display(Name = "Solution")]
        [AllowHtml]
        public string Solution { get; set; }

        [Display(Name = "Hiển thị thư viện")]
        public bool EnableGallery { get; set; }

        [Display(Name = "Enable capability")]
        public bool EnablePrintCapbility { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        #endregion

        #region Other properties

        public virtual List<Product> Products { get; set; }
        public virtual List<GalleryDetail> Galleries { get; set; }
        public virtual List<GalleryDetail> Prints { get; set; }

        private string _thumbPath;
        public virtual string ThumbPath
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

        public virtual int TotalGallery { get; set; }

        public virtual int TotalPrint { get; set; }                

        #endregion

    }


    public class CategorySettingsModel
    {
        public CategorySettingsModel()
        {
            this.PageSize = 12;
            this.EnableImage = false;
            this.ImageBackground = "#ffffff";
            this.ImageWidth = 200;
            this.ImageHeight = 200;
            this.EnableSorting = false;
            this.AllowErase = false;
        }

        public int Id { get; set; }

        public int PageSize { get; set; }

        public bool EnableImage { get; set; }

        public int ImageWidth { get; set; }

        public int ImageHeight { get; set; }

        public string ImageBackground { get; set; }

        public bool EnableSorting { get; set; }

        public bool AllowErase { get; set; }
    }
}
