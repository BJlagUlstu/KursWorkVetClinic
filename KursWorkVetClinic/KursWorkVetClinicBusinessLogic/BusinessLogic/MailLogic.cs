using KursWorkVetClinicBusinessLogic.HelperModels;
using System;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace KursWorkVetClinicBusinessLogic.BusinessLogic
{
    public class MailLogic
    {
        private static string smtpHost;
        private static int smtpPort;
        private static string mailLogin;
        private static string mailPassword;
        private static readonly string filename = "E:\\Report.pdf";
        public static void MailConfig(MailConfig config)
        {
            smtpHost = config.SmtpHost;
            smtpPort = config.SmtpPort;
            mailLogin = config.MailLogin;
            mailPassword = config.MailPassword;
        }
        public static async void MailSendAsync(MailSendInfo info)
        {
            if (string.IsNullOrEmpty(smtpHost) || smtpPort == 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(mailLogin) || string.IsNullOrEmpty(mailPassword))
            {
                return;
            }
            if (string.IsNullOrEmpty(info.MailAddress) || string.IsNullOrEmpty(info.Subject) || string.IsNullOrEmpty(info.Text))
            {
                return;
            }
            using (var objMailMessage = new MailMessage())
            {
                using (var objSmtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    try
                    {
                        Attachment attach = new Attachment(filename, MediaTypeNames.Application.Octet);
                        ContentDisposition disposition = attach.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(filename);
                        disposition.ModificationDate = File.GetLastWriteTime(filename);
                        disposition.ReadDate = File.GetLastAccessTime(filename);
                        objMailMessage.Attachments.Add(attach);
                        objMailMessage.From = new MailAddress(mailLogin);
                        objMailMessage.To.Add(new MailAddress(info.MailAddress));
                        objMailMessage.Subject = info.Subject;
                        objMailMessage.Body = info.Text;
                        objMailMessage.SubjectEncoding = Encoding.UTF8;
                        objMailMessage.BodyEncoding = Encoding.UTF8;
                        objSmtpClient.UseDefaultCredentials = false;
                        objSmtpClient.EnableSsl = true;
                        objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        objSmtpClient.Credentials = new NetworkCredential(mailLogin, mailPassword);
                        await Task.Run(() => objSmtpClient.Send(objMailMessage));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}