using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace ECommerce.Core.RabbitMQ;

public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQPublisher(IConfiguration configuration)
    {
        _configuration = configuration;

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
    
    public void Publish<T>(IDictionary<string, object> headers, T message)
    {
        var messageJson = JsonSerializer.Serialize(message);
        var messageBytes = Encoding.UTF8.GetBytes(messageJson);

        var exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
        _channel.ExchangeDeclare(exchangeName, ExchangeType.Headers, durable: true);

        var properties = _channel.CreateBasicProperties();
        properties.Headers = headers;
        
        var publicationAddress = new PublicationAddress(ExchangeType.Headers, exchangeName, string.Empty);
        
        _channel.BasicPublish(
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