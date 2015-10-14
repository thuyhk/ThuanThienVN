using HGT.Core;
using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HGT.Core.Utility;
using HGT.Entities;

namespace HGT.Web.Models
{
    public class NewsViewModel
    {
        public NewsViewModel()
        {
            //Pictures = new List<ProductPicture>();
        }

        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string MetaKey { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public Nullable<int> CountView { get; set; }
        public Nullable<bool> ShowHomePage { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Picture { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        #endregion

        #region Other properties

        private string _thumbnail;
        public string Thumbnail
        {
            get
            {
                if (string.IsNullOrEmpty(this.Picture))
                    return _thumbnail;
                else
                {
                    var url = Globals.ThumbPath(ConstantKeys.NewsFolder, this.Picture);
                    _thumbnail = url;
                }
                return _thumbnail;
            }
            set { _thumbnail = value; }
        }

        private string _pictureDefault;
        public string PictureDefault
        {
            get
            {
                if (string.IsNullOrEmpty(this.Picture))
                    return _pictureDefault;
                else
                {
                    var url = Globals.ImagePath(ConstantKeys.NewsFolder, this.Picture);
                    _pictureDefault = url;
                }
                return _pictureDefault;
            }
            set { _pictureDefault = value; }
        }


        //public IList<ProductPicture> Pictures { get; set; } // list pictures        

        #endregion


    }
}