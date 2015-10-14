using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using HGT.Service;
using HGT.Core;
using HGT.Admin.Extensions;
using HGT.Admin.Models;
using HGT.Core.Enums;
using HGT.Core.Utility;
using HGT.Core.Models;

namespace HGT.Admin.Controllers
{
    public class SettingController : AdminController
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingService _settingService;

        private UserInfo _userInfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;
        private static LogoModel _logoModel = new LogoModel();
        #endregion

        #region Constructors

        public SettingController(IUnitOfWork unitOfWork, ISettingService settingService)
        {
            this._unitOfWork = unitOfWork;
            this._settingService = settingService;
        }

        #endregion

        #region Utilities

        public ActionResult Index()
        {
            var model = new SettingModel();

            var setting = _settingService.Find((int)SettingType.SiteSetting);
            if (setting != null)
                model = SerializationUtility.DeserializeFromXml<SettingModel>(setting.Values);

            model = model ?? new SettingModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string EditSetting(SettingModel model, FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                var entity = _settingService.Find(model.Id);                
                if (entity != null)
               {                   
                    // clear cache
                    HgtCache.Remove(ConstantKeys.CacheSettingKey);

                    entity.Values = SerializationUtility.SerializeAsXml(model);
                    _settingService.Update(entity);
                    _unitOfWork.SaveChanges();

                    return JsonConvert.SerializeObject(new { ResultStatus.Success, Message = "Data were saved successfully!" });
                }
                return JsonConvert.SerializeObject(new { ResultStatus.Fail, Message = "Data were saved unsuccessfully!" });
            }
            return JsonConvert.SerializeObject(new { ResultStatus.Fail, Message = "Data were saved unsuccessfully!" });
        }

        public override ImageUploadModel ImageSetting()
        {
            return new ImageUploadModel()
            {
                ThumbPath = Globals.ThumbPath(""),
                ImagePath = Globals.ImageFolder,
                ImageWidth = 183,
                ImageHeight = 84,
                ThumbWidth = 183,
                ThumbHeight = 84,
                Quality = 100,
                AutoResize = false,
                AllowThumb = false,
                Folder = ""
            };
        }

        #endregion
    }
}