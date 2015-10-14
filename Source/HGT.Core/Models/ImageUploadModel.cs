using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Core.Models
{
    public class ImageUploadModel : BaseModel
    {        
        public string ImagePath { get; set; }
        public string ThumbPath { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int ThumbWidth { get; set; }
        public int ThumbHeight { get; set; }
        public int Quality { get; set; }
        public bool AllowThumb { get; set; }
        public string Folder { get; set; }
        public bool AutoResize { get; set; }

        
        
        
        
        public string ThumbUrl { get; set; }

        public string ImageUrl { get; set; }

        public string FileName { get; set; }
    }
}
