using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ECommerce.Infrastructure.dbcontext;

public class DapperDbContext
{
    public DapperDbContext(IConfiguration configuration)
    {
        var connectionStringTemplate = configuration.GetConnectionString("PostgreSQLConnection")!;
        var connectionString = connectionStringTemplate
            .Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
            .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
            .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
            .Replace("$POSTGRES_USERNAME", Environment.GetEnvironmentVariable("POSTGRES_USERNAME"))
            .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));
        
        DbConnection = new NpgsqlConnection(connectionString);
    }

    public IDbConnection DbConnection { get; }
}