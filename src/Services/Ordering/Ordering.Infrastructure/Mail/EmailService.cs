using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using System.Net.Mail;
using System.Net;
using System.Reflection;
using Ordering.Domain.Common;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmail(Email email)
        {

            MailMessage message = new MailMessage();

            message.From = new MailAddress(_emailSettings.FromAddress,_emailSettings.FromName);
            message.To.Add(new MailAddress(email.To));
            message.Subject = email.Subject;
            message.Body = email.Body;

            var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.FromAddress, _emailSettings.Password)
            };

            _logger.LogInformation("Email Sent");

            await client.SendMailAsync(message);
            /* if (message.DeliveryNotificationOptions == DeliveryNotificationOptions.OnSuccess)
                 return true;
             return false;*/
            return true;
        }
    }
}
