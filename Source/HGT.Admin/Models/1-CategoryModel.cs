using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using HGT.Admin.Extensions;
using HGT.Admin.Controllers;

namespace HGT.Admin.Models
{
    public class CategoryModel1
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [Display(Name = "Category alias")]
        public string Alias { get; set; }

        [Display(Name = "Meta key")]
        public string MetaKey { get; set; }

        [Display(Name = "Category root")]
        public int RootId { get; set; }

        private List<Category> _childrens { get; set; }
        public virtual List<Category> Childrens
        {
            get
            {
                if (_childrens != null)
                    return _childrens;

                using (var dtx = new HGTContext())
                {
                    var children = dtx.Categories.Where(x => x.RootId == this.Id ).ToList();
                    if (children.Count > 0)
                        _childrens = children;
                }
                return _childrens;
            }
            set { _childrens = value; }
        }

        private Category _parent;
        public virtual Category Parrent
        {

            get
            {
                if (_parent != null)
                    return _parent;

                using (var dtx = new HGTContext())
                {
                    var parent = dtx.Categories.Where(x => x.Id == this.RootId).FirstOrDefault();
                    if (parent != null)
                        _parent = parent;
                }
                return _parent;
            }
            set { _parent = value; }
        }

        public Nullable<int> Type { get; set; }

        public int DisplayOrder { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsHighlight { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        private string _categoryParrentName { get; set; }
        public string CategoryParrentName
        {
            get
            {
                if (!string.IsNullOrEmpty(_categoryParrentName))
                    return _categoryParrentName;
                if (this.Parrent != null)
                    _categoryParrentName = Parrent.Name;
                return _categoryParrentName;

            }
            set { _categoryParrentName = value; }
        }
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
