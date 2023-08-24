using Application.Interfaces.Email;
using Infrastructure.Services.Common;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services.Email {
    public class MailService : IMailService {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings) {
            _settings = settings.Value;
        }

        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default) {
            try {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
                mail.Sender = new MailboxAddress(_settings.DisplayName, _settings.From);
                mail.To.Add(MailboxAddress.Parse(mailData.To));

                var body = new BodyBuilder();
                mail.Subject = mailData.Subject;
                body.HtmlBody = mailData.Body;
                mail.Body = body.ToMessageBody();

                using var smtp = new SmtpClient();

                if (_settings.UseSSL) {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_settings.UseStartTls) {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                }
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);

                return true;
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
