using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Diabetes.Services
{   
        public class EmailService : IEmailService
        {
            private readonly EmailConfiguration _emailConfig;

            public EmailService(EmailConfiguration emailConfig)
            {
                _emailConfig = emailConfig;
            }

            public async Task SendVerificationEmail(string email, string verificationCode)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Diabetes App", _emailConfig.From));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Email Verification";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $"<p>Your verification code is: <strong>{verificationCode}</strong></p>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }   
}
