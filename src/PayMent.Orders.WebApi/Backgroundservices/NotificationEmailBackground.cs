using PayMents.Orders.Application.Abstractions;

namespace PayMent.Orders.WebApi.Backgroundservices;

public class NotificationEmailBackground
{
    private const string NOTIFICATIO_MESSAGE = "Ежедневное оповещение";
    
    private readonly IEmailService _emailService;
    private readonly IAccountsService _accountsService;

    public NotificationEmailBackground(IEmailService emailService, IAccountsService accountsService)
    {
        _emailService = emailService;
        _accountsService = accountsService;
    }

    public async Task PushEmailMessage()
    {
        var users = await _accountsService.GetAllAccountsAsync();
        foreach (var account in users)
        {
            await _emailService.SendEmailAsync(account.Email, NOTIFICATIO_MESSAGE);
        }
    }
}
