using HGT.Core;
using HGT.Core.Utility;
using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HGT.Admin.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            ListCategories = new List<SelectListItem>(); 
        }

        #region Properties

        public int Id { get; set; }
        
        [Display(Name = "Product code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Product name")]
        public string Name { get; set; }

        public string Alias { get; set; }

        [Display(Name = "Short description")]
        public string Description { get; set; }

        [Display(Name = "Content")]
        [AllowHtml]
        public string Content { get; set; }

        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Category name")]
        public int CategoryId { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Unit price")]
        public string Unit { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Show index")]
        public bool ShowIndex { get; set; }

        [Display(Name = "Origin")]
        public string Origin { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "Call price")]
        public bool CallPrice { get; set; }

        #endregion

        #region Other properties

        private string _thumbnail;
        public string Thumbnail
        {
            get
            {
                if (string.IsNullOrEmpty(this.Picture))
                    _thumbnail = Globals.AbsoluteImageDefault;
                else
                {
                    var url = Globals.ThumbPath(ConstantKeys.ProductFolder, this.Picture);
                    _thumbnail = url;
                }                
                return _thumbnail;
            }
            set { _thumbnail = value; }
        }

        public string _categoryName { get; set; }
        public string CategoryName { 
            get
            {
                if (_categoryName != null)
                    return _categoryName;
                using (var dtx = new HGTContext())
                {
                    var category = dtx.Categories.Find(this.CategoryId);
                    if (category != null)
                        _categoryName = category.Name;
                }
                return _categoryName;
            }
            set
            {
                _categoryName = value;
            }
        }   

        public IList<SelectListItem> ListCategories { get; set; }     

        #endregion
    }
}
