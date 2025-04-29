namespace ECommerce.Core.RabbitMQ;

public interface IRabbitMQPublisher
{
    void Publish<T>(IDictionary<string, object> routingKey, T message);
}