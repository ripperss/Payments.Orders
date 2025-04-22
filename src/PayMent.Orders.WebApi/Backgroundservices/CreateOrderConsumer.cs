using Microsoft.Extensions.Options;
using PayMents.Orders.Application.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace PayMent.Orders.WebApi.Backgroundservices;

public class CreateOrderConsumer : BackgroundService
{
    private readonly IOptions<RabbtiMqSettings> _settings;

    public CreateOrderConsumer(IOptions<RabbtiMqSettings> settings)
    {
        _settings = settings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var clinet = new ConnectionFactory()
        {
            HostName = _settings.Value.HostName,
            Port = _settings.Value.Port,
            Password = _settings.Value.Password        
        };

        var connection = await clinet.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += (_, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
        }; 
    }
}

