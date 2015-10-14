using HGT.Admin.Models;
using HGT.Core;
using HGT.Core.Enums;
using HGT.Core.Providers;
using HGT.Service;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HGT.Admin.Extensions;
using System.IO;
using Repository.Pattern.Infrastructure;
using System.Configuration;
using HGT.Entities.Models;
using HGT.Core.Utility;


namespace HGT.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: /Login/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailQueueService _emailQueueService;

        private UserInfo _userinfo = System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] as UserInfo;
        private bool _captchaShow = AppSettings.Settings.ShowCaptcha;

        public LoginController(IUnitOfWork unitOfWork, IUserService userService, IRoleService roleService,
            IEmailTemplateService emailTemplateService, IEmailQueueService emailQueueService)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._roleService = roleService;
            this._emailTemplateService = emailTemplateService;
            this._emailQueueService = emailQueueService;
        }

        //
        // GET: /Login/
        public ActionResult Index()
        {
            var model = new LoginModel();
            model.CaptchaShow = _captchaShow;
            if (!_captchaShow)
            {
                model.Captcha = "captcha";
            }
            return View(model);
        }

        // GET: /Login/
        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] = null;
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }
        public ActionResult Forgetpass()
        {
            var model = new ForgetPassModel();
            model.CaptchaShow = _captchaShow;
            if (!_captchaShow)
            {
                model.Captcha = "captcha";
            }
            return View(model);
        }
        [HttpPost]

        public ActionResult Forgetpass(ForgetPassModel mode)
        {
            if (_captchaShow)
            {
                if (!mode.Captcha.ToLower().Equals(Session[ConstantKeys.CaptchaSession].ToString().ToLower()))
                {
                    return Json(new { Status = 0, Message = "Enter security code not correct!" });
                }
            }

            string emailFrom = AppSettings.Settings.SMTPAccount;         
            string emailCc = ConfigurationManager.AppSettings["EmailCc"];
            string emailBcc = ConfigurationManager.AppSettings["EmailBcc"];
            string linkChangePass = ConfigurationManager.AppSettings["LinkChangePassword"];
            string siteUrl = AppSettings.Settings.SiteUrl.TrimEnd('/'); //ConfigurationManager.AppSettings["SiteUrl"].TrimEnd('/');

            Guid guid = Guid.NewGuid();
            var user = _userService.GetUserByEmail(mode.EmailTo);
            if (user != null)
            {
                string encyptToken = guid.ToString();//CryptorEngine.Encrypt(guid.ToString(), true);
                //string encyptEmail = CryptorEngine.Encrypt(mode.EmailTo, true);

                user.TokenForgotPassWord = encyptToken;
                _userService.Update(user);
                _unitOfWork.SaveChanges();

                // linkChangePass = string.Format(linkChangePass, siteUrl, encyptToken, encyptEmail);
                linkChangePass = string.Format(linkChangePass, siteUrl, encyptToken);

                var emailTemplateId = (int)EmailTemplateType.ForgetPass;
                var emaiTemplate = _emailTemplateService.GetEmailByID(emailTemplateId);

                var subject = emaiTemplate.Subject;
                string mailbody = emaiTemplate.Content;;
                mailbody = mailbody.Replace(ConstantKeys.UserName, user.UserName);
                mailbody = mailbody.Replace(ConstantKeys.ResetPasswordUrl, linkChangePass);
                mailbody = mailbody.Replace(ConstantKeys.SiteUrl, siteUrl);

                string ip = System.Web.HttpContext.Current.Request.UserHostAddress;

                var status = SendMailUtility.SendMailWithOutCC(subject, mailbody, mode.EmailTo, emailCc, emailBcc);

                var emailqueues = new EmailQueue();
                if (ModelState.IsValid)
                {
                    emailqueues.EmailFrom = emailFrom;
                    emailqueues.SendBy = user.UserId;
                    emailqueues.Active = status;
                    emailqueues.CreatedDate = DateTime.Now;
                    emailqueues.UpdatedDate = DateTime.Now;
                    emailqueues.EmailTo = mode.EmailTo;
                    emailqueues.EmailCc = emailCc;
                    emailqueues.EmailBcc = emailBcc;
                    emailqueues.EmailSubject = subject;
                    emailqueues.SenderIP = ip;
                    emailqueues.DisplayNameFrom = user.UserName;

                    _emailQueueService.Insert(emailqueues);
                    _unitOfWork.SaveChanges();
                }

            }
            else
            {
                return Json(new { Status = 0, Message = "Email not exist!" });
            }
            return Json(new { Status = 1, Message = "Please, check your email." });
        }

        [HttpPost]
        public ActionResult Logon(LoginModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_captchaShow)
                    {
                        if (!model.Captcha.ToLower().Equals(Session[ConstantKeys.CaptchaSession].ToString().ToLower()))
                        {
                            return Json(new { Status = 0, Message = "Enter security code not correct!" });
                        }
                    }
                    var userLogon = _userService.GetUserByUsername(model.UserName);
                    if (userLogon == null || userLogon.IsAdmin == false || userLogon.Active == false)
                    {
                        return Json(new { Status = 0, Message = "User name not correct!" });
                    }
                    //Encrypt password
                    var passwordEncryptInput = EncryptProvider.EncryptPassword(model.Password.Trim(), userLogon.PasswordSalt);

                    if (userLogon.Password.Equals(passwordEncryptInput))
                    {
                        _userinfo = new UserInfo();

                        _userinfo.ID = userLogon.UserId;
                        _userinfo.UserName = userLogon.UserName;
                        _userinfo.Email = userLogon.Email;
                        _userinfo.FullName = userLogon.FullName;
                        _userinfo.Image = userLogon.Image;
                        _userinfo.Active = userLogon.Active;
                                                
                        var roleDto = _roleService.Find(userLogon.RoleId);
                        //_userinfo.BitMask = new List<int>();
                        if (roleDto != null)
                        {
                            //int tempBitMask = GlobalFunctions.GetBitMaskOfUser(roleDto.MaskPermission);
                            //_userinfo.BitMask.Add(tempBitMask);

                            //check role
                            CheckPermUser(roleDto.MaskPermission, ref _userinfo);
                            _userinfo.MaskPermission = roleDto.MaskPermission;
                        }

                        System.Web.HttpContext.Current.Session[ConstantKeys.UserInfo] = _userinfo;

                        var UrlStr = Request.UrlReferrer.Query;
                        string UrlReturn = string.IsNullOrEmpty(UrlStr) ? "/admin/Dashboard/" : UrlStr.Split('=')[1];

                        return Json(new { Status = 1, ReturnUrl = (UrlReturn) });
                    }

                    else
                    {
                        return Json(new { Status = 0, Message = "User name or password not correct!" });

                    }

                }

            }
            catch (Exception)
            {
                return Json(new { Status = 0, Message = "User name or password not correct!" });
            }
            return Redirect("/");
        }

        public ActionResult Changepass(string id)
        {
            try
            {
                var model = new ChangePasswordModel();

                var user = _userService.GetUserByToken(id);
                if (user != null)
                {

                    model.EmailCF = user.Email;
                    model.TokenPassword = id;
                    return View(model);
                }
            }
            catch (Exception)
            {
                return Redirect("/Common");
            }
            return Redirect("/Common");
        }

        [HttpPost]
        public ActionResult ChangeNewPassword(ChangePasswordModel model)
        {
            try
            {
                var user = _userService.GetUserByEmail(model.EmailCF);
                if (model.TokenPassword.Equals(user.TokenForgotPassWord))
                {
                    string salt = EncryptProvider.GenerateSalt();
                    user.Password = EncryptProvider.EncryptPassword(model.Password, salt);
                    user.PasswordSalt = salt;
                    user.TokenForgotPassWord = null;
                    _userService.Update(user);
                    _unitOfWork.SaveChanges();

                    return Json(new { Status = 1, Message = "Change password success." });
                }
                else
                {
                    return Redirect("/Common");
                }

            }
            catch (Exception)
            {
                return Json(new { Status = 0, Message = "Change password error!" });

            }
        }

        /// <summary>
        /// Rilk:Check permission to add into user information variable
        /// </summary>
        /// <param name="bitMask">bitmask of user</param>
        /// <param name="usInfo">userinfor variables</param>

        private void CheckPermUser(long bitMask, ref UserInfo usInfo)
        {
            switch (bitMask)
            {
                case (int)RoleType.SA: usInfo.IsSA = true; break;
                case (int)RoleType.Administrator: usInfo.IsAdmin = true; break;
                case (int)RoleType.Manager: usInfo.IsManager = true; break;
                default: usInfo.IsUser = true; break;
            }
        }
        //check user email Exitst
        [HttpPost]
        public JsonResult DoesUserEmailExist(string email, int userId = 0)
        {
            var user = _userService.CheckDuplicatedEmail(email, userId);
            return Json(user);

        }

        public ActionResult GenerateCaptcha()
        {

            RandomImage randomimage = new RandomImage();
            string s = randomimage.GenerateRandomCode();
            Session[ConstantKeys.CaptchaSession] = s;
            FileContentResult imgstream = null;
            RandomImage img = new RandomImage(s, 200, 50);
            var mem = new MemoryStream();
            img.Image.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
            imgstream = this.File(mem.GetBuffer(), "image/Jpeg");
            img.Dispose();
            return imgstream;
        }

    }
}
