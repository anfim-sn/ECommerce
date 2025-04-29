using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMqProductDeleteConsumer : IRabbitMqProductDeleteConsumer, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;
    private readonly ILogger<RabbitMqProductDeleteConsumer> _logger;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMqProductDeleteConsumer(IConfiguration configuration, IDistributedCache cache, ILogger<RabbitMqProductDeleteConsumer> logger)
    {
        _configuration = configuration;
        _cache = cache;
        _logger = logger;
        Console.WriteLine($"RabbitMQ HostName: {_configuration["RabbitMQ_HostName"]}");
        Console.WriteLine($"RabbitMQ Port: {_configuration["RabbitMQ_Port"]}");
        Console.WriteLine($"RabbitMQ UserName: {_configuration["RabbitMQ_UserName"]}");
        Console.WriteLine($"RabbitMQ Password: {_configuration["RabbitMQ_Password"]}");
        
        var hostName = _configuration["RabbitMQ_HostName"];
        var port = _configuration["RabbitMQ_Port"];
        var username = _configuration["RabbitMQ_UserName"];
        var password = _configuration["RabbitMQ_Password"];

        var connectionFactory = new ConnectionFactory
        {
            HostName = hostName,
            Port = int.Parse(port.Contains(':') ? port.Split(':').Last() : port),
            UserName = username,
            Password = password,
        };
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Consume()
    {
        var queueName = "orders.product.delete.queue";

        var headers = new Dictionary<string, object>()
        {
            { "x-match", "all" },
            { "event", "product.delete" },
            { "RowCount", 1 }
        };
        
        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Headers, durable: true);
        
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queueName, exchangeName, string.Empty, arguments: headers);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (sender, args) => {
            var bodyArr = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(bodyArr);

            if (message != null)
            {
                var productNameUpdateMessage = JsonSerializer.Deserialize<ProductDeleteMessage>(message);
                var productKey = $"product:{productNameUpdateMessage?.ProductId}";
                
                await _cache.RemoveAsync(productKey);
                
                _logger.LogInformation($"Product Has Been Deleted in cache: {productNameUpdateMessage?.ProductId}");
            }
            
        };
        
        _channel.BasicConsume(queueName, autoAck: true, consumer);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}