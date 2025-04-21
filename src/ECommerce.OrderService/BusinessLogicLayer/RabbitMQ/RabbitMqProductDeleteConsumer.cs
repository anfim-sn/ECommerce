using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMqProductDeleteConsumer : IRabbitMqProductDeleteConsumer, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqProductDeleteConsumer> _logger;
    private readonly IChannel _channel;
    private readonly IConnection _connection;

    public RabbitMqProductDeleteConsumer(IConfiguration configuration, ILogger<RabbitMqProductDeleteConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;

        var hostName = _configuration["RabbitMQ_HostName"];
        var port = _configuration["RabbitMQ_Port"];
        var username = _configuration["RabbitMQ_UserName"];
        var password = _configuration["RabbitMQ_Password"];

        var connectionFactory = new ConnectionFactory
        {
            HostName = hostName,
            Port = int.Parse(port),
            UserName = username,
            Password = password,
        };
        _connection = connectionFactory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
    }

    public async Task ConsumeAsync()
    {
        var routingKey = "product.delete";
        var queueName = "orders.product.delete.queue";
        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true);
        await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        await _channel.QueueBindAsync(queueName, exchangeName, routingKey);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, args) => {
            var bodyArr = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(bodyArr);

            if (message != null)
            {
                var productNameUpdateMessage = JsonSerializer.Deserialize<ProductDeleteMessage>(message);
                _logger.LogInformation($"Product Has Been Deleted: {productNameUpdateMessage?.ProductId}");
            }
            
        };
        
        await _channel.BasicConsumeAsync(queueName, autoAck: true, consumer);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}