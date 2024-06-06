using System.Net.Mail;
using System.Net;

namespace FAK.Infrastructure.Services
{
    public interface IEmailSender
    {
        void SendEmail(string fromEmail, string toEmail, string subject, string HtmlMessage);
    }
    public class EmailSender : IEmailSender
    {
        public async void SendEmail(string fromEmail, string toEmail, string subject, string HtmlMessage)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com", //or another email sender provider
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("fajararifkurniawan2807@gmail.com", "jrhciaxpybcmqoor")
            };

            await client.SendMailAsync(fromEmail, toEmail, subject, HtmlMessage);
        }
    }
}
