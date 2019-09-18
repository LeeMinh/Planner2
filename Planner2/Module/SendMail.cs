using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Planner2.Module
{
    public class SendMail
    {

        public static void SendEmail(List<string> _email, string Subject, string body, string Link = "", string Title = "", int Type = 0, string[] _cc = null)
        {
            if (string.IsNullOrEmpty(Title))
            {
                Title = Common.SettingData.TenCongTy;
            }
            Task.Run(() =>
            {
                string senderID = Common.SettingData.SendMail_Email;
                string senderPassword = Common.SettingData.SendMail_Password;
                MailMessage mail = new MailMessage();
                try
                {
                    mail.Bcc.Add("huychu.k14@gmail.com");
                    if (_cc != null)
                    {
                        foreach (var item in _cc)
                        {
                            mail.CC.Add(item);
                        }
                    }
                    if (Title != "")
                    {
                        mail.From = new MailAddress(senderID, Title);
                    }
                    else
                    {
                        mail.From = new MailAddress(senderID);

                    }
                    mail.From = new MailAddress(senderID, Title);
                    mail.Subject = Subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = Common.SettingData.SendMail_Host; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                    smtp.Port = int.Parse(Common.SettingData.SendMail_Port);
                    smtp.EnableSsl = true;
                    Attachment attachment = null;
                    if (Link != "")
                    {
                        attachment = new Attachment(Link);
                        mail.Attachments.Add(attachment);
                    }
                    smtp.Send(mail);

                    return true;
                }
                catch (Exception ex)
                {
                    mail.To.Add("huychu.k14@gmail.com");

                    if (Title != "")
                    {
                        mail.From = new MailAddress(senderID, Title);
                    }
                    else
                    {
                        mail.From = new MailAddress(senderID);

                    }
                    mail.From = new MailAddress(senderID, Title);
                    mail.Subject = Subject;
                    mail.Body = ex.ToString() + "<br>" + Link + "<br>" + body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = Common.SettingData.SendMail_Host; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential(senderID, senderPassword);
                    smtp.Port = int.Parse(Common.SettingData.SendMail_Port);
                    smtp.EnableSsl = true;

                    smtp.Send(mail);


                    return false;
                }
            });
        }
    }

}