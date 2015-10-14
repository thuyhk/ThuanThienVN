using HGT.Core.Enums;
using HGT.Core.Extensions;
using HGT.Core.Models;
using HGT.Core.Utility;
using HGT.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGT.Core
{
    public class AppSettings
    {
        private static Object objLock = new Object();
        private const string SettingKey = ConstantKeys.CacheSettingKey;

        public static string GetSetting()
        {

            var dtx = new HGTContext();
            var result = dtx.Settings.Find((int)SettingType.SiteSetting).Values;
            return result;
        }

        public static SettingModel ReloadSetting()
        {
            var settings = HgtCache.Get<SettingModel>(SettingKey);
            if (settings == null)
            {
                lock (objLock)
                {
                    string settingXml = "";
                    //AppSettings temptSettings = new AppSettings();
                    try
                    {
                        settingXml = GetSetting();
                        settings = SerializationUtility.DeserializeFromXml<SettingModel>(settingXml);
                        if(settings == null)
                        {
                            settings = new SettingModel();

                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                    HgtCache.Insert(SettingKey, settings);
                }
            }

            return settings;

        }

        public static SettingModel Settings
        {
            get
            {
                return ReloadSetting();
            }
        }

        public static void StartAppSettings()
        {
            ReloadSetting();
        }

        //public AppSettings()
        //{
        //    this.Id = (int)SettingType.SiteSetting;
        //    this.SiteActive = true;
        //    this.SiteName = string.Empty;
        //    this.SiteUrl = string.Empty;
        //    this.MetaKeyword = string.Empty;
        //    this.MetaDescription = string.Empty;
        //    this.ShowCaptcha = false;

        //    this.CompanyName = "C/o";
        //    this.CompanyAddress = "xxxxxxxxxxxxxxxxxxxxxx";
        //    this.BankAccountNo = "xxxxxxxxxxxxx";
        //    this.EmailAdmin = string.Empty;
        //    this.CompanyPhone = "xx xx xx xx";
        //    this.CompanyFaxNo = "xx xx xx xx";

        //    this.HostServer = "smtp.gmail.com";
        //    this.Port = 587;
        //    this.SMTPAuthentication = true;
        //    this.SMTPAccount = string.Empty;
        //    this.SMTPPassword = string.Empty;
        //    this.EnableSSL = true;

        //    this.AllowOpengraph = true;
        //    this.OgContent = "";
        //    this.FacebookLink = "http://www.facebook.com/sharer.php?u={0}";
        //    this.TwitterLink = "http://twitter.com/intent/tweet?status={0}";
        //    this.GoogleLink = "https://plus.google.com/share?url={0}";
        //    this.ShareScript = "";
        //}

        //#region properties

        //// general settings
        //public int Id { get; set; }
        //public bool SiteActive { get; set; }
        //public string SiteName { get; set; }

        //private string _siteUrl;
        //public string SiteUrl
        //{
        //    get
        //    {
        //        if (!String.IsNullOrEmpty(HgtContext.Current.GetCurrentSiteURL()))
        //        {
        //            return HgtContext.Current.GetCurrentSiteURL();
        //        }
        //        return _siteUrl;
        //    }
        //    set { _siteUrl = value; }
        //}

        //public string MetaKeyword { get; set; }
        //public string MetaDescription { get; set; }
        //public bool ShowCaptcha { get; set; }
        //public string LogoURL { get; set; }

        //// company info
        //public string CompanyName { get; set; }
        //public string CompanyIntro { get; set; }
        //public string CompanyAddress { get; set; }
        //public string Country { get; set; }
        //public string CompanyPhone { get; set; }
        //public string CompanyFaxNo { get; set; }
        //public string CompanyEmail { get; set; }
        //public string BankAccountNo { get; set; }
        //public string EmailAdmin { get; set; }
        //public string EmailOrder { get; set; }

        //// email setting
        //public string HostServer { get; set; }
        //public int Port { get; set; }
        //public bool SMTPAuthentication { get; set; }
        //public string SMTPAccount { get; set; }
        //public string SMTPPassword { get; set; }
        //public bool EnableSSL { get; set; }

        ////social networks
        //public bool AllowOpengraph { get; set; }
        //public string OgContent { get; set; }
        //public string ShareScript { get; set; }
        //public string FacebookLink { get; set; }
        //public string GoogleLink { get; set; }
        //public string TwitterLink { get; set; }

        //#endregion
    }
}
