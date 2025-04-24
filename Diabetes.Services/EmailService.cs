using System;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Diabetes.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailConfiguration> emailConfig, ILogger<EmailService> logger)
        {
            _emailConfig = emailConfig.Value;
            _logger = logger;
        }

        public async Task SendVerificationEmail(string email, string verificationCode)
        {
            using var client = new SmtpClient();
            try
            {
                var message = CreateEmailMessage(email, verificationCode);
                await ConfigureAndSendEmail(client, message);
                _logger.LogInformation($"Email sent successfully to {email}");
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, $"SMTP Command Error while sending to {email}. StatusCode: {ex.StatusCode}");
                throw new EmailSendException("SMTP error occurred", ex);
            }
            catch (SmtpProtocolException ex)
            {
                _logger.LogError(ex, $"SMTP Protocol Error while sending to {email}");
                throw new EmailSendException("Protocol error occurred", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"General error sending email to {email}");
                throw new EmailSendException("Failed to send verification email", ex);
            }
            finally
            {
                try
                {
                    if (client.IsConnected)
                    {
                        await client.DisconnectAsync(true);
                    }
                }
                catch (Exception disconnectEx)
                {
                    _logger.LogWarning(disconnectEx, "Error disconnecting SMTP client");
                }
            }
        }

        private MimeMessage CreateEmailMessage(string email, string verificationCode)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Diabetes App", _emailConfig.From));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Email Verification - Diabetes App";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h2>Diabetes App Verification</h2>
                    <p>Your verification code is: <strong>{verificationCode}</strong></p>
                    <p>This code will expire in 24 hours.</p>
                    <p>If you didn't request this, please ignore this email.</p>"
            };

            message.Body = bodyBuilder.ToMessageBody();
            return message;
        }

        private async Task ConfigureAndSendEmail(SmtpClient client, MimeMessage message)
        {
            client.Timeout = 30000;
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await client.ConnectAsync(
                _emailConfig.SmtpServer,
                _emailConfig.Port,
                GetSecureSocketOption(_emailConfig.Port));

            if (!string.IsNullOrEmpty(_emailConfig.UserName))
            {
                await client.AuthenticateAsync(
                    _emailConfig.UserName,
                    _emailConfig.Password);
            }

            await client.SendAsync(message);
        }

        private MailKit.Security.SecureSocketOptions GetSecureSocketOption(int port)
        {
            return port switch
            {
                465 => MailKit.Security.SecureSocketOptions.SslOnConnect,
                587 => MailKit.Security.SecureSocketOptions.StartTls,
                _ => MailKit.Security.SecureSocketOptions.Auto
            };
        }
    }

    public class EmailSendException : Exception
    {
        public EmailSendException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}