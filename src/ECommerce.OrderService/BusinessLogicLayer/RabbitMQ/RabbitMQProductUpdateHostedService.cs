using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQProductUpdateHostedService(IRabbitMqProductUpdateConsumer rabbitMqProductUpdateConsumer) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        rabbitMqProductUpdateConsumer.Consume();
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        rabbitMqProductUpdateConsumer.Dispose();
        return Task.CompletedTask;
    }
}