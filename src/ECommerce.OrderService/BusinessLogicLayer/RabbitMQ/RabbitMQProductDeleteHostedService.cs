using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQProductDeleteHostedService(IRabbitMqProductDeleteConsumer rabbitMqProductNameUpdateConsumer) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        rabbitMqProductNameUpdateConsumer.Consume();
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        rabbitMqProductNameUpdateConsumer.Dispose();
        return Task.CompletedTask;
    }
}