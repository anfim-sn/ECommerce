using ECommerce.Core.Entities;
using ECommerce.Core.RepositoryContracts;
using ECommerce.Infrastructure.dbcontext;

namespace ECommerce.Infrastructure.Repositories;

internal class ProductsRepository(DapperDbContext DbContext) : IProductRepository
{
    public Task<List<Product>> GetListAsync()
    {
        throw new NotImplementedException();
    }
    public Task<Product> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
    public Task<List<Product>> SearchAsync(string searchString)
    {
        throw new NotImplementedException();
    }
    public Task<object> AddAsync(Product map)
    {
        throw new NotImplementedException();
    }
    public Task<object> UpdateAsync(Product map)
    {
        throw new NotImplementedException();
    }
    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}