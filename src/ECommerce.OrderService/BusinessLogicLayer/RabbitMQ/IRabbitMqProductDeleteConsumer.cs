namespace BusinessLogicLayer.RabbitMQ;

public interface IRabbitMqProductDeleteConsumer
{
    void Consume();
    void Dispose();
}