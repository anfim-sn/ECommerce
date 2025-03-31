using System.Reflection;
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
        
        return services;
    }
}