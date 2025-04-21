
using ECommerce.Core.Entities;

namespace ECommerce.Core.RepositoryContracts;

public interface IProductRepository
{
    Task<List<Product>> GetListAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Product? GetByCondition(Func<Product, bool> predicate, bool tracked);
    Task<List<Product>> SearchAsync(string searchString);
    Task<int> AddAsync(Product product);
    Task<int> UpdateAsync(Product product);
    Task<bool> DeleteByIdAsync(Guid id);
}