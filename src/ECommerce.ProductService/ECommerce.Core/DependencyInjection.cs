using ECommerce.Core.RabbitMQ;
using ECommerce.Core.ServiceContracts;
using ECommerce.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension Method to add core services to the DI container
    /// </summary>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IProductsService, ProductsService>();
        services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
        
        return services;
    }
}