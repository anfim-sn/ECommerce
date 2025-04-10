using ECommerce.Core.RepositoryContracts;
using ECommerce.Infrastructure.dbcontext;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extension Method to add infrastructure services to the DI container
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IProductRepository, ProductsRepository>();

        services.AddDbContextFactory<ProductDbContext>((provider, options) => {
            var connectionStringTemplate = provider.GetRequiredService<IConfiguration>()
                .GetConnectionString("PostgreSQLConnection")!;
            var connectionString = connectionStringTemplate
                .Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
                .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
                .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
                .Replace("$POSTGRES_USERNAME", Environment.GetEnvironmentVariable("POSTGRES_USERNAME"))
                .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));
            options.UseNpgsql(connectionString, o => o.MigrationsAssembly("ECommerce.Infrastructure"));
        });
        
        return services;
    }
}