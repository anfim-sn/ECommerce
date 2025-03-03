using ECommerce.Core.RepositoryContracts;
using ECommerce.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extension Method to add infrastructure services to the DI container
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, UsersRepository>();

        return services;
    }
}