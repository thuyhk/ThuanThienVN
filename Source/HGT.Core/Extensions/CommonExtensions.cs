using HGT.Core.Enums;
using HGT.Core.Models;
using HGT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace HGT.Core.Extensions
{    
    public class CommonExtensions
    {
        //public static UploadStatus UploadPhoto(HttpPostedFileBase image, out ImageUploadModel imageUploaded, int width, int height, int thumbWidth, int thumbHeight, int quality, string imagePath, string imageThumbPath, bool allowThumb, bool autoCreateThumb, int? idPrefix, bool useGuid = true)
        //{
        //    if (string.IsNullOrEmpty(image.FileName))
        //        throw new ArgumentNullException("fileName");

        //    var file = new FileInfo(image.FileName);
        //    imageUploaded = new ImageUploadModel();
        //    if (!ImageUtility.IsImageExtension(file.Extension))
        //        return UploadStatus.WrongExtensions;
        //    //return Json(new { Status = ActionResultStatus.Fail, Message = "banner_InvalidExtention,bannerrotators".Localize("Invalid image file. Support File ext: jpg, jpeg, gif, png, bmp.") }, "text/html");

        //    var path = imagePath.EndsWith("/") ? imagePath : imagePath + "/";

        //    var fileNameGenerated = string.Format("{0}{1}", Globals.GenerateAlias(image.FileName.Replace(file.Extension, string.Empty)), file.Extension);

        //    if (useGuid || string.Compare(fileNameGenerated, file.Extension, true) == 0)
        //        fileNameGenerated = string.Format("{0}{1}", Guid.NewGuid().ToString().Trim('-'), file.Extension);

        //    if (idPrefix.HasValue && idPrefix.Value > 0)
        //        fileNameGenerated = string.Format("{0}-{1}", idPrefix.Value, fileNameGenerated);

        //    var tempPath = string.Format("temp_{0}", fileNameGenerated);
        //    var imgTempPath = string.Format("{0}", fileNameGenerated);

        //    if (!Directory.Exists(Globals.MapPath(path)))
        //        Directory.CreateDirectory(Globals.MapPath(path));

        //    var tempFilePath = Globals.MapPath(string.Format("{0}{1}", path, tempPath));
        //    var imageTempFilePath = Globals.MapPath(string.Format("{0}{1}", path, imgTempPath));

        //    if (tempFilePath.Length > 200)
        //        return UploadStatus.FileNameTooLong;
        //    //return Json(new { Status = ActionResultStatus.Fail, Message = "error_UploadFileNameTooLong, bannerrotators".Localize("Upload file name is too long!") }, "text/plain");.

        //    image.SaveAs(tempFilePath);

        //    using (var bmp = ImageUtility.FixResizeImage(tempFilePath, width, height, Color.White))
        //    using (var eps = new EncoderParameters(1))
        //    {
        //        eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

        //        var ici = ImageUtility.GetCodecByExt(file.Extension);

        //        if (image.ContentLength / 1000 > 100)
        //            bmp.Save(imageTempFilePath, ici, eps);
        //        else
        //            bmp.Save(imageTempFilePath, ici, eps);
        //    }

        //    if (!allowThumb || autoCreateThumb)
        //    {
        //        var thumbPath = imageThumbPath.EndsWith("/") ? imageThumbPath : imageThumbPath + "/";

        //        if (!Directory.Exists(Globals.MapPath(thumbPath)))
        //            Directory.CreateDirectory(Globals.MapPath(thumbPath));

        //        var tempFileThumbPath = Globals.MapPath(string.Format("{0}{1}", thumbPath, tempPath));
        //        image.SaveAs(tempFileThumbPath);

        //        var pathThumbtemp = string.Format("{0}", fileNameGenerated);
        //        var thumbTempFilePath = Globals.MapPath(thumbPath + pathThumbtemp);

        //        using (var bmpThumb = ImageUtility.CreateThumbnail(tempFileThumbPath, thumbWidth, thumbHeight, Color.White))
        //        {
        //            using (var eps = new EncoderParameters(1))
        //            {
        //                eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

        //                var ici = ImageUtility.GetCodecByExt(file.Extension);

        //                if (image.ContentLength / 1000 > 100)
        //                    bmpThumb.Save(thumbTempFilePath, ici, eps);
        //                else
        //                    bmpThumb.Save(thumbTempFilePath, ici, eps);
        //            }
        //        }
        //        if (File.Exists(tempFileThumbPath))
        //            File.Delete(tempFileThumbPath);

        //        imageUploaded.ThumbUrl = string.Format("{0}{1}", imageThumbPath.EndsWith("/") ? imageThumbPath : imageThumbPath + "/", pathThumbtemp);
        //        imageUploaded.ImageUrl = string.Format("{0}{1}", imagePath.EndsWith("/") ? imagePath : imagePath + "/", imgTempPath);
        //        imageUploaded.FileName = fileNameGenerated;
        //    }

        //    if (File.Exists(tempFilePath))
        //        File.Delete(tempFilePath);

        //    imageUploaded.ThumbUrl = (string.Format("{0}{1}", imageThumbPath.EndsWith("/") ? imageThumbPath : imageThumbPath + "/", fileNameGenerated))
        //        .Replace("~", string.Empty);
        //    imageUploaded.ImageUrl = (string.Format("{0}{1}", imagePath.EndsWith("/") ? imagePath : imagePath + "/", fileNameGenerated))
        //        .Replace("~", string.Empty);
        //    imageUploaded.FileName = fileNameGenerated;

        //    return UploadStatus.Success;
        //}

    }


}
