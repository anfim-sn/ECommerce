namespace BusinessLogicLayer.RabbitMQ;

public interface IRabbitMqProductDeleteConsumer
{
    Task ConsumeAsync();
    void Dispose();
}