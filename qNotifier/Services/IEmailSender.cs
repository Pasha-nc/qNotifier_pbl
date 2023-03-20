using System.Net;
using System.Net.Mail;

namespace qNotifier.Services
{
    public interface IEmailSender
    {
        public void SendEmail(string userEmail, string subject, string emailContent);
    }

    public class EmailSender : IEmailSender
    {
        public void SendEmail(string userEmail, string subject, string emailContent)
        {
            string emailPass = Environment.GetEnvironmentVariable("str1");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            SmtpClient smtp = new SmtpClient("mail.qnotifier.com");

            smtp.EnableSsl = false;
            smtp.Port = 25;
            smtp.Credentials = new NetworkCredential("admin@qnotifier.com", emailPass);

            MailMessage message = new MailMessage();

            message.Sender = new MailAddress("admin@qnotifier.com", "qNotifier");
            message.From   = new MailAddress("admin@qnotifier.com", "qNotifier");

            message.To.Add(new MailAddress(userEmail, userEmail));

            message.Subject = subject;
            message.Body = emailContent;

            message.IsBodyHtml = true;
            smtp.Send(message);
        }
    }
}
