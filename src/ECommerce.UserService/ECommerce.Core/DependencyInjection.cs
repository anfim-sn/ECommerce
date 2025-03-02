using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension Method to add core services to the DI container
    /// </summary>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {

        return services;
    }
}