namespace ECommerce.Core.RabbitMQ;

public interface IRabbitMQPublisher
{
    Task PublishAsync<T>(IDictionary<string, object> routingKey, T message);
}