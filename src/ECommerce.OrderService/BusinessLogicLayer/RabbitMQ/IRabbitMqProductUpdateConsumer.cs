namespace BusinessLogicLayer.RabbitMQ;

public interface IRabbitMqProductUpdateConsumer
{
    Task ConsumeAsync();
    void Dispose();
}