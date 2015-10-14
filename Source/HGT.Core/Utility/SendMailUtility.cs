using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;

namespace HGT.Core.Utility
{
    public static class SendMailUtility
    {
        public static bool SendMailWithCCAndBcc(string cusName, string mailfrom, string mailpass, string host, int port, string subj, string message, 
            bool enablessl, string toemail = "", string cc = "", string bcc = "")
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(mailfrom, cusName);
                mailMessage.Subject = subj;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                string[] ToMultiEmail = toemail.Split(',');
                foreach (string ToEMailId in ToMultiEmail)
                {
                    mailMessage.To.Add(new MailAddress(ToEMailId));
                }
                if (cc.Length > 4)
                {
                    string[] CCId = cc.Split(',');
                    foreach (string CCEmail in CCId)
                    {
                        mailMessage.CC.Add(new MailAddress(CCEmail));
                    }
                }
                if (bcc.Length > 5)
                {
                    string[] BCCId = bcc.Split(',');
                    foreach (string BCCEmailId in BCCId)
                    {
                        mailMessage.Bcc.Add(new MailAddress(BCCEmailId));
                    }
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = host;
                smtp.EnableSsl = enablessl;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = mailMessage.From.Address;
                NetworkCred.Password = mailpass;
                smtp.Credentials = NetworkCred;
                smtp.Port = port;
                smtp.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public static bool SendMailWithOutCC(string cusName, string subject, string content, string emailTo, string mailCC = "", string mailBCC = "")
        {
            var appSettings = AppSettings.Settings;
            string emailFrom = appSettings.SMTPAccount;
            string emailPassword = appSettings.SMTPPassword;
            string host = appSettings.HostServer;
            string port = appSettings.Port.ToString(); 
            bool enableSsl = appSettings.EnableSSL;

            var status = SendMailWithCCAndBcc(cusName, emailFrom, emailPassword, host, Convert.ToInt32(port), subject, content, enableSsl, emailTo, mailCC, mailBCC);
            return status;
        }
    }
}