using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
namespace FinanceFlow.Infrastructure.Serivces
{
    public class EmailService 
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void Send(string mailTo, string subject, string message, bool isHtml = false)
        {
            var myEmail = configuration["EmailSettings:SenderEmail"];
            var appPassword = configuration["EmailSettings:Password"];

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(myEmail, appPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(myEmail, mailTo, subject, message)
            {
                IsBodyHtml = isHtml
            };

            client.Send(mailMessage);
        } 
    }
}