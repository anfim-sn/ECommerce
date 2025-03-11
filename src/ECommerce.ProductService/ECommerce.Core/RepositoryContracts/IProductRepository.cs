
using ECommerce.Core.Entities;

namespace ECommerce.Core.RepositoryContracts;

public interface IProductRepository
{
    Task<List<Product>> GetListAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> SearchAsync(string searchString);
    Task<int> AddAsync(Product product);
    Task<int> UpdateAsync(Product product);
    Task<bool> DeleteByIdAsync(Guid id);
}