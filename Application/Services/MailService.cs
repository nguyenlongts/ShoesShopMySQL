using System.Net;
using System.Net.Mail;
using ShoesShop.Application.Interfaces.Services;

namespace ShoesShop.Application.Services
{
    public class MailService : IEmailService
    {
        private readonly IConfiguration _config;
        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMailAsync(string address, string subject, string body)
        {
            var smtpHost = _config["Smtp:Host"];
            var smtpPort = _config["Smtp:Port"];
            var smtpUsername = _config["Smtp:Username"];
            var smtpPassword = _config["Smtp:Password"];

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpPort) || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
            {
                throw new InvalidOperationException("SMTP configuration is missing or incomplete.");
            }

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = int.Parse(smtpPort),
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            var mail = new MailMessage
            {
                From = new MailAddress("noreply@shoesshop.com", "ShoesShop Support"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mail.To.Add(address);

            await smtpClient.SendMailAsync(mail);
        }
    }
}
