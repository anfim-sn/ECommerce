namespace ECommerce.Core.RabbitMQ;

public interface IRabbitMQPublisher
{
    Task PublishAsync<T>(string routingKey, T message);
}