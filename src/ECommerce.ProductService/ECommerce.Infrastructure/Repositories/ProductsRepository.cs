using ECommerce.Core.RepositoryContracts;
using ECommerce.Infrastructure.dbcontext;

namespace ECommerce.Infrastructure.Repositories;

internal class ProductsRepository(DapperDbContext DbContext) : IProductRepository
{
    // public async Task<Product?> AddUser(Product user)
    // {
    //     user.UserId = Guid.NewGuid();
    //
    //     const string query = "INSERT INTO \"public\".\"Users\" (\"UserId\", \"Email\", \"PersonName\", \"Gender\", \"Password\") " +
    //                          "VALUES (@UserId, @Email, @PersonName, @Gender, @Password);";
    //
    //     var rowAffected = await DbContext.DbConnection.ExecuteAsync(query, user);
    //     
    //     return rowAffected > 0 ? user : null;
    // }
}