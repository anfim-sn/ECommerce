using System.Text;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMqProductUpdateConsumer : IRabbitMqProductUpdateConsumer, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;
    private readonly ILogger<RabbitMqProductUpdateConsumer> _logger;
    private readonly IChannel _channel;
    private readonly IConnection _connection;

    public RabbitMqProductUpdateConsumer(IConfiguration configuration, IDistributedCache cache, ILogger<RabbitMqProductUpdateConsumer> logger)
    {
        _configuration = configuration;
        _cache = cache;
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
        var queueName = "orders.product.update.queue";

        var headers = new Dictionary<string, object>()
        {
            { "x-match", "all" },
            { "event", "product.update" },
            { "RowCount", 1 }
        };
        
        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Headers, durable: true);
        
        await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        await _channel.QueueBindAsync(queueName, exchangeName, string.Empty, arguments: headers);
        
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, args) => {
            var bodyArr = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(bodyArr);
            
            _logger.LogInformation($"Message received: {message}");
            
            if (message != null)
            {
                var productNameUpdateMessage = JsonSerializer.Deserialize<ProductDTO>(message);
                var productKey = $"product:{productNameUpdateMessage?.ProductId}";
                var isCachedProductExist = await _cache.GetStringAsync(productKey) != null;

                if (isCachedProductExist)
                {
                    await _cache.SetStringAsync(productKey, message, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300)
                    });
                    
                    _logger.LogInformation($"Product Name Has Been Updated in cache: {productNameUpdateMessage?.ProductId}");
                }
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