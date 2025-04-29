namespace BusinessLogicLayer.RabbitMQ;

public interface IRabbitMqProductUpdateConsumer
{
    void Consume();
    void Dispose();
}