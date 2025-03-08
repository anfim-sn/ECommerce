using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ECommerce.Infrastructure.dbcontext;

public class DapperDbContext
{
    public DapperDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQLConnection");
        DbConnection = new NpgsqlConnection(connectionString);
    }

    public IDbConnection DbConnection { get; }
}