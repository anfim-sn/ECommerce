using Dapper;
using ECommerce.Core.DTO;
using ECommerce.Core.Entities;
using ECommerce.Core.RepositoryContracts;
using ECommerce.Infrastructure.dbcontext;

namespace ECommerce.Infrastructure.Repositories;

internal class UsersRepository(DapperDbContext DbContext) : IUserRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();

        const string query = "INSERT INTO \"public\".\"Users\" (\"UserId\", \"Email\", \"PersonName\", \"Gender\", \"Password\") " +
                             "VALUES (@UserId, @Email, @PersonName, @Gender, @Password);";

        var rowAffected = await DbContext.DbConnection.ExecuteAsync(query, user);
        
        return rowAffected > 0 ? user : null;
    }
    
    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password)
    {
        const string query = $"SELECT * FROM \"public\".\"Users\" WHERE \"Email\" = @Email AND \"Password\" = @Password;";
        var param = new { Email = email, Password = password };

        var result = await DbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, param);

        return result;
    }
}