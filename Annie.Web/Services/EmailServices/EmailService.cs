using Annie.Web.Models.Core;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Annie.Web.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        public EmailService(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
        }

        public async Task SendMailAsync(Message emailMessage)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(_appSettings.EmailSettings.MailboxName, _appSettings.EmailSettings.Sender));
            mimeMessage.To.Add(MailboxAddress.Parse(emailMessage.Email));
            mimeMessage.Subject = emailMessage.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailMessage.Text
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_appSettings.EmailSettings.HostForConnect, int.Parse(_appSettings.EmailSettings.PortForConnect), (MailKit.Security.SecureSocketOptions)int.Parse(_appSettings.EmailSettings.SecureSocketOption));
                    client.AuthenticationMechanisms.Remove("XOAUTH2"); // Must be removed for Gmail SMTP
                    await client.AuthenticateAsync(_appSettings.EmailSettings.AuthenticateUserName, _appSettings.EmailSettings.AuthenticatePassword);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    // TODO: запись в лог
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }



    public abstract class Message
    {
        public abstract string Email { get; set; }
        public abstract string Subject { get; set; }
        public abstract string Text { get; set; }
    }

    public static class EmailMessages
    {
        public sealed class NewMessage : Message
        {
            public NewMessage(string email, string subject, string text)
            {
                this.Email = email;
                this.Subject = subject;
                this.Text = text;
            }

            public override string Email { get; set; }
            public override string Subject { get; set; }
            public override string Text { get; set; }
        }

        public sealed class ConfirmRegistrationMessage : Message
        {
            public ConfirmRegistrationMessage(string recipientEmail, string registrationConfirmKey)
            {
                this.Email = recipientEmail;
                this.Subject = "Подтверждение регистрации";
                this.Text = $@"Здравствуйте! <br /> Вы получили это письмо, потому что зарегистрировались на сайте www.1zvonok.com. <br />
                               Для подтверждения регистрации, пожалуйста, пройдите по ссылке: { GetLinkForComfirmEmail(registrationConfirmKey) } <br />
                               Если Вы получили это письмо по ошибке, просто проигнорируйте его. <br />
                               Письмо было отправлено автоматически, отвечать на него не нужно. <br />
                               С уважением, { GetHost() }";
            }

            public override string Email { get; set; }
            public override string Subject { get; set; }
            public override string Text { get; set; }

            private string GetLinkForComfirmEmail(string registrationConfirmKey) => GetHost() + "/confirm-registration/" + registrationConfirmKey;
            private string GetHost() => "https://1zvonok.com";
            //private string GetLinkForComfirmEmail(string registrationConfirmKey) => GetHost() + "/confirm-registration/" + registrationConfirmKey;
            //private string GetHost() => _httpContextAccessor.HttpContext.Request.Host.Host;
        }
    }
}
