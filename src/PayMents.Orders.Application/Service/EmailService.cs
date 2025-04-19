using Microsoft.Extensions.Options;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Settings;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace PayMents.Orders.Application.Service;

public class EmailService : IEmailService
{
    private readonly IOptions<EmailSettings> _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings;
    }

    public async Task SendEmailAsync(string email, string subject)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("отправитель", _settings.Value.Email));
        message.To.Add(new MailboxAddress("Получатель", email));
        message.Subject = "оповещение";

        var body = new BodyBuilder();
        body.TextBody = subject;

        message.Body = body.ToMessageBody();

        using(var client = new SmtpClient())
        {
            await client.ConnectAsync(
                _settings.Value.SmtpServer
                , _settings.Value.SmptpPort
                , SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(_settings.Value.Email, _settings.Value.Password);
            await client.SendAsync(message);
        }
    }
}
