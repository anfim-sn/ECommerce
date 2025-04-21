namespace BusinessLogicLayer.RabbitMQ;

public interface IRabbitMqProductNameUpdateConsumer
{
    Task ConsumeAsync();
    void Dispose();
}