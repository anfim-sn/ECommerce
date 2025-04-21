using System.Reflection;
using BusinessLogicLayer.RabbitMQ;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(OrderAddRequestValidator).Assembly);

        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        services.AddScoped<IOrderService, OrderService>();

        services.AddStackExchangeRedisCache(o => o.Configuration = $"{configuration["REDIS_HOST"]}:{configuration["REDIS_PORT"]}");

        services.AddTransient<IRabbitMqProductNameUpdateConsumer, RabbitMqProductNameUpdateConsumer>();
        services.AddHostedService<RabbitMQProductNameUpdateHostedService>();
        
        return services;
    }
}