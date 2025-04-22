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
    
    public async Task PublishAsync<T>(IDictionary<string, object> headers, T message)
    {
        var messageJson = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(messageJson);

        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Headers, durable: true);

        var properties = new BasicProperties { Headers = headers };

        var publicationAddress = new PublicationAddress(ExchangeType.Headers, exchangeName, string.Empty);
        
        await _channel.BasicPublishAsync<BasicProperties>(
            addr: publicationAddress,
            basicProperties: properties, 
            body: messageBytes);
    }
    
    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}