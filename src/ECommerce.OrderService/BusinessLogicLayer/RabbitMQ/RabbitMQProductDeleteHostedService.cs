using Microsoft.Extensions.Hosting;

namespace BusinessLogicLayer.RabbitMQ;

public class RabbitMQProductDeleteHostedService(IRabbitMqProductDeleteConsumer rabbitMqProductNameUpdateConsumer) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await rabbitMqProductNameUpdateConsumer.ConsumeAsync();
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        rabbitMqProductNameUpdateConsumer.Dispose();
        await Task.CompletedTask;
    }
}