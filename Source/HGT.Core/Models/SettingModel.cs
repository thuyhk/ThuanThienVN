using HGT.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using HGT.Core;

namespace HGT.Core.Models
{
    public class SettingModel
    {
        public SettingModel()
        {
            this.Id = (int)SettingType.SiteSetting;
            this.SiteActive = true;
            this.SiteName = string.Empty;
            this.SiteUrl = string.Empty;
            this.MetaKeyword = string.Empty;
            this.MetaDescription = string.Empty;
            this.ShowCaptcha = false; 

            this.CompanyName = "C/o";
            this.CompanyAddress = "xxxxxxxxxxxxxxxxxxxxxx";
            this.BankAccountNo = "xxxxxxxxxxxxx";
            this.EmailAdmin = string.Empty;
            this.CompanyPhone = "xx xx xx xx";
            this.CompanyFaxNo = "xx xx xx xx";

            this.HostServer = "smtp.gmail.com";
            this.Port = 587;
            this.SMTPAuthentication = true;
            this.SMTPAccount = string.Empty;
            this.SMTPPassword = string.Empty;
            this.EnableSSL = true;

            this.AllowOpengraph = true;
            this.OgContent = "";
            this.FacebookLink = "http://www.facebook.com/sharer.php?u={0}";
            this.TwitterLink = "http://twitter.com/intent/tweet?status={0}";
            this.GoogleLink = "https://plus.google.com/share?url={0}";
            this.ShareScript = "";
        }

        public int Id { get; set; }

        #region general

        [DisplayName("Site active")]
        public bool SiteActive { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Site name")]
        public string SiteName { get; set; }

        //[Required(ErrorMessage = "* Required")]
        [DisplayName("Site Url")]
        public string SiteUrl { get; set; }

        [DisplayName("Meta keyword")]
        public string MetaKeyword { get; set; }

        [DisplayName("Meta description")]
        public string MetaDescription { get; set; }

        [DisplayName("Enable captcha")]
        public bool ShowCaptcha { get; set; }

        [DisplayName("Upload logo")]
        public string LogoURL { get; set; }

        private string _thumbPath;
        public string ThumbPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.LogoURL))
                    _thumbPath = Globals.AbsoluteImageDefault;
                else
                {
                    var url = Globals.AbsoluteImagePath("", this.LogoURL);
                    _thumbPath = url;
                }
                return _thumbPath;
            }
            set { _thumbPath = value; }
        }

        public virtual string ImageUpload { get; set; }

        #endregion

        #region information
        [Required(ErrorMessage = "* Required")]
        [DisplayName("Company name")]
        public string CompanyName { get; set; }

        [DisplayName("Company address")]
        public string CompanyAddress { get; set; }

        [DisplayName("Introduction")]
        public string CompanyIntro { get; set; }

        [DisplayName("Company email")]
        public string CompanyEmail { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        [DisplayName("Company phone")]
        public string CompanyPhone { get; set; }

        [DisplayName("Company fax")]
        public string CompanyFaxNo { get; set; }

        public string BankAccountNo { get; set; }

        [DisplayName("Email admin")]
        public string EmailAdmin { get; set; }

        [DisplayName("Email order")]
        public string EmailOrder { get; set; }

        #endregion

        #region email

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Host server")]
        public string HostServer { get; set; }

        public int Port { get; set; }

        [DisplayName("SMTP authentication")]
        public bool SMTPAuthentication { get; set; }

        [DisplayName("SMTP account")]
        public string SMTPAccount { get; set; }

        [DisplayName("SMTP password")]
        public string SMTPPassword { get; set; }

        [DisplayName("Enable SSL")]
        public bool EnableSSL { get; set; }

        #endregion

        #region social network

        [DisplayName("Allow Opengraph tags")]
        public bool AllowOpengraph { get; set; }

        [AllowHtml]
        [DisplayName("Opengraph content")]
        public string OgContent { get; set; }

        [AllowHtml]
        [DisplayName("Share script")]
        public string ShareScript { get; set; }

        [DisplayName("Facebook")]
        public string FacebookLink { get; set; }

        [DisplayName("Google")]
        public string GoogleLink { get; set; }

        [DisplayName("Twitter")]
        public string TwitterLink { get; set; }




        //        <meta property="og:title" content="{title}" />
        //<meta property="og:site_name" content="{site_name}" />
        //<meta property="og:description" content="{summary}" />
        //<meta property="og:url" content="{url}" />
        //<meta property="og:image" content="{image}" />

        #endregion

        #region hotmail
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        #endregion

    }
}
