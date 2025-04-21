using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace ECommerce.Core.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IChannel _channel;
    private readonly IConnection _connection;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
        
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
    
    public async Task PublishAsync<T>(string routingKey, T message)
    {
        var messageJson = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(messageJson);

        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true);

        await _channel.BasicPublishAsync(exchangeName, routingKey, messageBytes);
    }
    
    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}