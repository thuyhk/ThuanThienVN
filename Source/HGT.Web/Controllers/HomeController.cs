using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using HGT.Core;
using HGT.Core.Models;
using HGT.Core.Enums;
using HGT.Core.Utility;
using HGT.Core.Extensions;
using HGT.Entities.Models;
using HGT.Service;
using HGT.Web.Models;
using HGT.Core.Enumerations;
using System.IO;

namespace HGT.Web.Controllers
{
    public class HomeController : Controller
    {
        #region cache key
        private const string BANNER_SLIDER_ALL_KEY = "Banner.Slider.All";
        private const string CONTENT_CONTACT_INFO_KEY = "Content.Contact.Info";
        #endregion

        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        private readonly IBannerService _bannerService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IEmailQueueService _emailQueueService;
        private readonly IContentService _contactService;
        private readonly IProductService _productService;
        private readonly ISolutionService _solutionService;

        private bool _captchaShow = AppSettings.Settings.ShowCaptcha;

        #endregion

        #region Contructors

        public HomeController(IUnitOfWork unitOfWork, ICategoryService categoryService,
            IBannerService bannerService, IEmailQueueService emailQueueService,
            IEmailTemplateService emailTemplateService, IContentService contentService,
            IProductService productService, ISolutionService solutionService)
        {
            this._unitOfWork = unitOfWork;
            this._categoryService = categoryService;
            this._bannerService = bannerService;
            this._emailQueueService = emailQueueService;
            this._emailTemplateService = emailTemplateService;
            this._contactService = contentService;
            this._productService = productService;
            this._solutionService = solutionService;
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            var lst = _categoryService.ODataQueryable().ToList();
            var products = _productService.ODataQueryable().ToList();
            return View();
        }

        public ActionResult Introduction()
        {
            var content = _contactService.Find((int)ContentType.Introduction);
            return View(content);
        }

        #region contact page
        public ActionResult Contact()
        {
            var infoContact = new ContentModel();
            infoContact = HgtCache.Get<ContentModel>(CONTENT_CONTACT_INFO_KEY);
            if (infoContact == null)
            {
                var contact = _contactService.Find((int)ContentType.Contact);
                infoContact = contact.ToModel();
                HgtCache.Insert(CONTENT_CONTACT_INFO_KEY, infoContact);
            }

            var model = new ContactModel() { Departments = GetAllDepartments() };
            ViewData["ContactInfo"] = infoContact.Value;
            model.CaptchaShow = _captchaShow;
            if (!_captchaShow)
            {
                model.Captcha = "captcha";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                if (_captchaShow)
                {

                    if (!model.Captcha.ToLower().Equals(Session[ConstantKeys.CaptchaSession].ToString().ToLower()))
                    {
                        return Json(new { Status = ResultStatus.CaptchaInCorrect, Message = StringTable.CaptchaInCorrect });
                    }
                }

                string emailFrom = AppSettings.Settings.SMTPAccount;
                string emailCc = AppSettings.Settings.EmailOrder; // +"," + model.ContactEmail;
                string emailTo = AppSettings.Settings.CompanyEmail;

                var emailTemplateId = (int)EmailTemplateType.ContactForm;
                var emaiTemplate = _emailTemplateService.GetEmailByID(emailTemplateId);

                // get subject of email template
                var subject = emaiTemplate.Subject;

                // format body email
                string mailbody = emaiTemplate.Content;
                mailbody = mailbody.Replace(ConstantKeys.UserName, model.ContactName);
                mailbody = mailbody.Replace(ConstantKeys.FromEmail, model.ContactEmail);
                mailbody = mailbody.Replace(ConstantKeys.ContactPhone, model.ContactPhone);
                mailbody = mailbody.Replace(ConstantKeys.CompanyName, "<strong>Công ty:</strong> " + model.CompanyName);

                var department = GetAllDepartments().Where(x => x.Value == model.DepartmentId.ToString()).Select(x => x.Text).FirstOrDefault();
                mailbody = mailbody.Replace(ConstantKeys.Department, department);
                mailbody = mailbody.Replace(ConstantKeys.Content, model.ContactContent);
                mailbody = mailbody.Replace(ConstantKeys.CreatedDate, DateTime.Now.ToLongTimeString() + " - " + DateTime.Now.ToShortDateString());

                // send mail to email admin
                var status = SendMailUtility.SendMailWithOutCC(model.ContactName, "Liên hệ mới", mailbody, emailTo, emailCc);

                if (status)
                {
                    // save info sendmail
                    //string ip = System.Web.HttpContext.Current.Request.UserHostAddress;
                    //var emailqueues = new EmailQueue();
                    //emailqueues.EmailFrom = emailFrom;
                    //emailqueues.Department = department;
                    //emailqueues.Active = false;
                    //emailqueues.EmailTo = model.ContactEmail;
                    //emailqueues.EmailCc = emailFrom;
                    //emailqueues.EmailSubject = subject;
                    //emailqueues.SenderIP = ip;
                    //emailqueues.DisplayNameFrom = model.ContactName;
                    //emailqueues.PhoneContact = model.ContactPhone;
                    //emailqueues.EmailContact = model.ContactEmail;
                    //emailqueues.CompanyName = model.CompanyName;
                    //emailqueues.CreatedDate = DateTime.Now;
                    //emailqueues.UpdatedDate = DateTime.Now;

                    //_emailQueueService.Insert(emailqueues);
                    //_unitOfWork.SaveChanges();

                    return Json(new { Status = ResultStatus.Success, Message = "Gửi liên hệ thành công!" });
                }
                return Json(new { Status = ResultStatus.Fail, Message = "Gửi liên hệ không thành công!" });
            }
        }

        #endregion

        #region slider banner
        public ActionResult _Slider()
        {
            var model = new List<BannerModel>();
            model = HgtCache.Get<List<BannerModel>>(BANNER_SLIDER_ALL_KEY);
            if (model == null)
            {
                var banners = _bannerService.ODataQueryable().Where(x => x.IsActive == true).OrderBy(x => x.DisplayOrder).ThenBy(x => x.Title).ToList();
                model = banners.ToListModel();
                HgtCache.Insert(BANNER_SLIDER_ALL_KEY, model);
            }
            return PartialView(model);
        }

        #endregion

        #region service
        public ActionResult _Services()
        {
            var query = _solutionService.ODataQueryable().Where(x => x.IsActive == true && x.IsFeature == true).Take(4).OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name).ToList();
            var list = query.ToListModel();
            return PartialView(list);
        }

        public ActionResult ServicePage()
        {
            var query = _solutionService.ODataQueryable().Where(x => x.IsActive == true).OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name).ToList();
            var list = query.ToListModel();
            return View(list);
        }

        public ActionResult SolutionDetail(string Alias)
        {
            var entity = _solutionService.ODataQueryable().Where(x => x.Alias == Alias.Trim()).FirstOrDefault();
            var model = entity.ToModel();
            model.Products = _productService.GetListProductBySolutionId(model.Id);
            return View(model);
        }

        #endregion

        #region #others
        public List<SelectListItem> GetAllDepartments()
        {
            var results = new List<SelectListItem>();
            results.Add(new SelectListItem() { Value = ((int)Department.Product).ToString(), Text = "Sản phẩm Domino" });
            results.Add(new SelectListItem() { Value = ((int)Department.InkAndSpares).ToString(), Text = "Mực in & phụ kiện" });
            results.Add(new SelectListItem() { Value = ((int)Department.ServiceAndSupport).ToString(), Text = "Dịch vụ & hỗ trợ" });
            results.Add(new SelectListItem() { Value = ((int)Department.TrainingAndManual).ToString(), Text = "Đào tạo & hướng dẫn sử dụng" });
            return results;
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
        #endregion

        #endregion
    }
}