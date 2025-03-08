
using ECommerce.Core.Entities;

namespace ECommerce.Core.RepositoryContracts;

public interface IProductRepository
{
    Task<List<Product>> GetListAsync();
    Task<Product> GetByIdAsync(int id);
    Task<List<Product>> SearchAsync(string searchString);
    Task<object> AddAsync(Product map);
    Task<object> UpdateAsync(Product map);
    Task<bool> DeleteByIdAsync(int id);
}