using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;

namespace Web.UI.Helper
{
    public class MailHelper
    {
        public static string SendMail(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                MailMessage mail = new MailMessage
                {
                    IsBodyHtml = isHtml,
                    From = GetFrom(),
                    Subject = subject,
                    Body = body
                };
                string[] tos = to.Split(",;".ToCharArray());
                foreach (string t in tos)
                {
                    mail.To.Add(t);
                }

                var setting = GetSettings();
                SmtpClient cc = new SmtpClient(setting.Network.Host, setting.Network.Port);
                cc.Credentials = new NetworkCredential(setting.Network.UserName, setting.Network.Password);
                cc.Send(mail);

            }
            catch (SmtpException sex)
            {
                return sex.GetMessage();
            }
            catch (System.Exception ex)
            {
                return ex.GetMessage();
            }
            return string.Empty;
        }

        public static string SendMail(string to, string subject, string body, params MailAttach[] attaches)
        {
            try
            {
                MailMessage mail = new MailMessage
                {
                    SubjectEncoding = Encoding.GetEncoding(1254),
                    BodyEncoding = Encoding.GetEncoding(1254),
                    IsBodyHtml = true,
                    From = GetFrom(),
                    Subject = subject,
                    Body = body
                };

                foreach (var item in attaches)
                {
                    Attachment a = new Attachment(item.Content, item.Name, item.MimeType);
                    mail.Attachments.Add(a);
                }

                string[] tos = to.Split(",;".ToCharArray());
                foreach (string t in tos)
                {
                    mail.To.Add(t);
                }
                Sender(mail);
            }
            catch (SmtpException sex)
            {
                return sex.GetMessage();
            }
            catch (System.Exception ex)
            {
                return ex.GetMessage();
            }
            return string.Empty;
        }


        public static MailAddress GetFrom()
        {
            return new MailAddress(GetSettings().From, ApplicationSettingHelper.BrandName);
        }

        public static SmtpSection GetSettings()
        {
            return (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        }

        static void Sender(MailMessage mail)
        {
            var setting = GetSettings();
            SmtpClient cc = new SmtpClient(setting.Network.Host, setting.Network.Port);
            cc.Credentials = new NetworkCredential(setting.Network.UserName, setting.Network.Password);
            cc.Send(mail);
        }

        public static bool IsValidEmailAddress(string email)
        {
            return !string.IsNullOrEmpty(email) && System.Text.RegularExpressions.Regex.IsMatch(email, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
        }
        public static void SendMailTooMaintainers(string body, string subject)
        {
            try
            {
                string[] toList = ConfigurationManager.AppSettings["Maintainer"].Split(";".ToCharArray());
                foreach (var to in toList)
                    SendMail(to, subject, body);
            }
            catch (Exception)
            {
            }
        }

        public class MailAttach
        {
            public Stream Content { get; set; }
            public string Name { get; set; }
            public string MimeType { get; set; }
        }
    }
}