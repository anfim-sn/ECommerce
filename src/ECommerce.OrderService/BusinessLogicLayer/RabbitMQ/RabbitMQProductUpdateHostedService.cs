using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQProductUpdateHostedService(IRabbitMqProductUpdateConsumer rabbitMqProductUpdateConsumer) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await rabbitMqProductUpdateConsumer.ConsumeAsync();
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        rabbitMqProductUpdateConsumer.Dispose();
        await Task.CompletedTask;
    }
}