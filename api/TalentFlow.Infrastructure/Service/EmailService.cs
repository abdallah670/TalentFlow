using Microsoft.Extensions.Options;

using MimeKit;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Models;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace TalentFlow.Infrastructure.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings settings;
        public EmailService(IOptions<EmailSettings> options)
        {
            settings = options.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(settings.Email));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart("html")
            {
                Text = body
            };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(settings.Host, settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(settings.Email, settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
