using HGT.Admin.Filters;
using HGT.Core;
using HGT.Core.Enums;
using HGT.Core.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HGT.Admin.Extensions;
using HGT.Core.Models;

namespace HGT.Admin.Controllers
{
    [AuthFilter]
    public class AdminController : Controller
    {
        public AdminController()
        {

        }

        public virtual ImageUploadModel ImageSetting()
        {
            return new ImageUploadModel()
                {
                    ThumbPath = Globals.TempThumbFolder,
                    ImagePath = Globals.TempFolder,
                    ImageHeight = 300,
                    ImageWidth = 400,
                    ThumbWidth = 200,
                    ThumbHeight = 150,
                    Quality = 80,
                    AutoResize = true,
                    AllowThumb = true,
                    Folder = ""
                };
        }

        #region Fields
        protected Color defaultBackground = Color.White;
        protected bool enabledThumb = true;
        protected bool? autoCreateThumb = true;
        protected int defaultThumbWidth = 50;
        protected int defaultThumbHeight = 50;
        protected int defaultWidth = 100;
        protected int defaultHeight = 100;
        protected int imageQuality = 90; //0 to 100 
        #endregion

        #region TryCreatePath

        protected bool TryCreatePath(string path)
        {
            var mapPath = Globals.MapPath(path);
            if (Directory.Exists(mapPath)) return false;
            else
            {
                Directory.CreateDirectory(mapPath);
                return true;
            }
        }

        #endregion

        #region TryDeleteFile
        protected bool TryDeleteFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region TryMoveFile
        protected bool TryMoveFile(string srcFile, ref string dstFile, bool skipInject = false)
        {
            if (!System.IO.File.Exists(srcFile)) return false;

            var tmp = dstFile;
            int iseq = 1;

            while (!skipInject && System.IO.File.Exists(tmp))
            {
                tmp = InjectFilePath(dstFile, iseq++);
            }

            IOUtility.CopyFile(srcFile, dstFile = tmp, true);
            IOUtility.DeleteFile(srcFile);

            return true;
        }
        #endregion

        #region InjectFilePath

        protected string InjectFilePath(string filePath, int iseq)
        {
            return string.Format("{0}{1}{2}({3}){4}", Path.GetDirectoryName(filePath), '\\',
                Path.GetFileNameWithoutExtension(filePath), iseq, Path.GetExtension(filePath));
        }

        #endregion

        #region SaveFile

        protected void SaveFile(HttpPostedFileBase file, ref string filePath)
        {
            var tmp = filePath;
            int iseq = 1;

            while (System.IO.File.Exists(tmp))
            {
                tmp = InjectFilePath(filePath, iseq++);
            }

            file.SaveAs(filePath = tmp);
        }

        #endregion

        #region ResizeImage

        protected void ResizeImage(string srcFilePath, ref string dstFilePath, int width, int height, Color background,
            int quality = 90)
        {
            Bitmap bmp = ImageUtility.FixResizeImage(srcFilePath, width, height, background);

            var tmp = dstFilePath;
            int iseq = 1;

            while (System.IO.File.Exists(tmp))
            {
                tmp = InjectFilePath(dstFilePath, iseq++);
            }

            bmp.Save(dstFilePath = tmp, quality);
            bmp.Dispose();
        }

        #endregion

        #region CreateImageThumb

        protected void CreateImageThumb(string srcFilePath, ref string dstFilePath, int width, int height,
            Color background, int quality = 90)
        {
            Bitmap bmpThumb = ImageUtility.CreateThumbnail(srcFilePath, width, height, background);

            var tmp = dstFilePath;
            int iseq = 1;

            //while (System.IO.File.Exists(tmp))
            //{
            //    tmp = InjectFilePath(dstFilePath, iseq++);
            //}

            bmpThumb.Save(dstFilePath = tmp, quality);
            bmpThumb.Dispose();
        }

        #endregion

        #region NormalizeFileName

        protected string NormalizeFileName(string file, object id)
        {
            var name = Path.GetFileNameWithoutExtension(file) ?? "no_name";

            if (name.Length > 50) name = name.Substring(0, 50);
            var ret = Globals.GenerateAlias(name) + Path.GetExtension(file);

            if (id != null) ret = string.Format("{0}_{1}", id, ret);

            return ret;
        }

        #endregion

        #region JsonUpload
        protected JsonResult JsonUpload(object data)
        {
            HttpContext cxt;
            if (HgtContext.Current != null && null != (cxt = HgtContext.Current.Context))
            {
                if (cxt.Request != null && cxt.Request.AcceptTypes != null &&
                    cxt.Request.AcceptTypes.Any(x => x.EqualsIgCase("application/json")))
                {
                    return Json(data, "text/html");
                }
            }

            return Json(data, "text/plain");
        }
        #endregion

        #region DoUploadImage

        protected bool DoUploadImage(object id, ImageUploadModel model, HttpPostedFileBase image, out string imagePath)
        {
            string thumbPath;
            if (DoUploadImage(id, model, image, out imagePath, out thumbPath, true))
            {
                if (!string.IsNullOrEmpty(thumbPath))
                    imagePath = thumbPath;                
                return true;
            }
            else return false;
        }

        protected bool DoUploadImage(object id, ImageUploadModel model, HttpPostedFileBase image, out string imagePath, out string thumbPath, bool createThumb)
        {
            imagePath = thumbPath = null;

            if (!ImageUtility.IsImageExtension(Path.GetExtension(image.FileName))) return false;

            string tmpModDir = model.ImagePath;
            string tmpThumbDir = model.ThumbPath;
            string fileName = NormalizeFileName(Path.GetFileName(image.FileName), id);

            TryCreatePath(tmpModDir);

            string tmpFilePath = Globals.MapPath(tmpModDir.CombineWPath("temp_" + fileName));
            string tmpImageFile = Globals.MapPath(tmpModDir.CombineWPath(fileName));

            SaveFile(image, ref tmpFilePath);

            int width = model.ImageWidth, height = model.ImageHeight;
            if (!model.AutoResize)
            {
                using (var img = System.Drawing.Image.FromFile(tmpFilePath))
                {
                    width = img.Width; height = img.Height;
                }
            }


            ResizeImage(tmpFilePath, ref tmpImageFile, width, height, defaultBackground, model.Quality);

            imagePath = tmpModDir.CombineWPath(Path.GetFileName(tmpImageFile)).TrimStart('~');

            if (model.AllowThumb)
            {
                TryCreatePath(tmpThumbDir);
                string tmpThumbFile = Globals.MapPath(Globals.ThumbPath(model.Folder, Path.GetFileName(tmpImageFile)));
                CreateImageThumb(tmpFilePath, ref tmpThumbFile, model.ThumbWidth, model.ThumbHeight, defaultBackground, model.Quality);
                thumbPath = tmpThumbDir.CombineWPath(Path.GetFileName(tmpThumbFile)).TrimStart('~');
            }

            TryDeleteFile(tmpFilePath);

            return true;
        }

        #endregion

        #region UploadImage

        [HttpPost]
        public ActionResult UploadImage(int id, HttpPostedFileBase imageUpload)
        {
            var imageSetting = ImageSetting();

            if (imageUpload == null) return JsonStatus("Upload image", false);

            string imagePath;
            if (DoUploadImage(id, imageSetting, imageUpload, out imagePath))
            {
                return JsonUpload(new
                {
                    Status = ResultStatus.Success,
                    Message = string.Empty,
                    ImagePath = imagePath,
                    FileName = Path.GetFileName(imagePath),
                    FileExt = Path.GetExtension(imagePath).TrimStart('.')
                });
            }
            else return JsonUpload(new
            {
                Status = ResultStatus.Fail,
                Message = "Invalid image extension. Supported extensions (*.jpg, *.jpeg, *.gif, *.png, *.bmp)"
            });
        }

        #endregion

        //#region DoUploadFile

        protected bool DoUploadFile(object id, HttpPostedFileBase file, out string filePath)
        {
            string fileName = NormalizeFileName(Path.GetFileName(file.FileName), id);

            //TryCreatePath(Globals.TempFolder);

            filePath = Globals.MapPath("~/userfiles/attachments/" + fileName);

            SaveFile(file, ref filePath);

            //filePath = ModuleUrl.TempAttachmentPath(Path.GetFileName(filePath)).TrimStart('~');

            return true;
        }

        //#endregion

        //#region UploadFile

        [HttpPost]
        public ActionResult UploadFile(int id, HttpPostedFileBase FileAttach)
        {
            if (FileAttach == null) return JsonStatus("Upload document file!", false);

            string filePath;
            if (DoUploadFile(id, FileAttach, out filePath))
            {
                return JsonUpload(new
                {
                    Status = ResultStatus.Success,
                    Message = string.Empty,
                    FilePath = filePath,
                    FileName = Path.GetFileName(filePath),
                    FileExt = Path.GetExtension(filePath).TrimStart('.'),
                    MimeType = FileAttach.ContentType
                });
            }
            else return JsonUpload(new
            {
                Status = ResultStatus.Fail,
                Message = "Error!"
            });
        }

        //#endregion

        //#region MoveFile

        ///// <summary>
        ///// return final file name
        ///// </summary>
        //protected string MoveFile(string permfolder, string tmpFolder, string fileUrl, string permThumbfolder = null)
        //{
        //    var fileName = MoveFileEx(permfolder, tmpFolder, fileUrl);

        //    if (permThumbfolder == null) permThumbfolder = ModuleUrl.ThumbFolder;

        //    MoveFileEx(permThumbfolder, ModuleUrl.TempThumbFolder, fileUrl, fileName);

        //    return fileName;
        //}

        //protected string MoveFileEx(string permfolder, string tmpFolder, string fileUrl, string newFileName = null)
        //{
        //    var dstfileName = newFileName;
        //    if (dstfileName == null)
        //    {
        //        dstfileName = Path.GetFileNameWithoutExtension(fileUrl);
        //        dstfileName = Regex.Replace(dstfileName ?? "", @"\(\d+\)$", "");
        //        if (dstfileName.Length > 0) dstfileName += Path.GetExtension(fileUrl);
        //        else dstfileName = Path.GetFileName(fileUrl);
        //    }

        //    var srcPath = Globals.MapPath(tmpFolder.CombineWPath(Path.GetFileName(fileUrl)));
        //    var dstPath = Globals.MapPath(permfolder.CombineWPath(dstfileName));

        //    TryCreatePath(permfolder);

        //    TryMoveFile(srcPath, ref dstPath, newFileName != null);

        //    return Path.GetFileName(dstPath);
        //}

        //#endregion

        #region JsonStatus

        protected JsonResult JsonStatus(string action, bool isSuccess)
        {
            return Json(new
            {
                Status = (isSuccess ? ResultStatus.Success : ResultStatus.Fail),
                Message = string.Format("{0} {1}", action, (isSuccess ? "Success" : "Unsuccess").ToString().ToLower())
            });
        }

        #endregion

    }
}
