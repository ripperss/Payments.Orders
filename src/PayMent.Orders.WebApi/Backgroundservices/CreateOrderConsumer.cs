using Microsoft.Extensions.Options;
using PayMents.Orders.Application.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using PayMents.Orders.Application.Models.Orders;
using PayMents.Orders.Application.Abstractions;


namespace PayMent.Orders.WebApi.Backgroundservices;

public class CreateOrderConsumer : BackgroundService
{
    private readonly IOptions<RabbtiMqSettings> _settings;
    private readonly IServiceScopeFactory _scopeFactory;

    public CreateOrderConsumer(
          IOptions<RabbtiMqSettings> settings
        , IServiceScopeFactory scopeFactory)
    {
        _settings = settings;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await CreateChannelAsync();

        await Createqueue(channel);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
                
            var createOrderDto = JsonSerializer.Deserialize<CreateOrderDto>(message)
            ?? throw new ArgumentNullException("заказ не может быть равен Null");

            using var scope = _scopeFactory.CreateScope();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

            await orderService.Create(createOrderDto);
        };

        await channel
            .BasicConsumeAsync(
            _settings.Value.queueName
            , autoAck: false, consumer
            , cancellationToken: stoppingToken);
    }

    private async Task Createqueue(IChannel channel)
    {
        await channel.QueueDeclareAsync(
            queue: _settings.Value.queueName,
            durable: true,       
            exclusive: false,
            autoDelete: false,
            arguments: null
            );
    }

    private async Task<IChannel> CreateChannelAsync()
    {
        var clinet = new ConnectionFactory()
        {
            HostName = _settings.Value.HostName,
            Port = _settings.Value.Port,
            Password = _settings.Value.Password
        };

        var connection = await clinet.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        return channel;
    }
}

