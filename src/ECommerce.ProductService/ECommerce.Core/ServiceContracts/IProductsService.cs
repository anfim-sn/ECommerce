
using ECommerce.Core.DTO;
using ECommerce.Core.Entities;

namespace ECommerce.Core.ServiceContracts;

public interface IProductsService
{
    Task<List<Product>> GetListAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<List<Product>> SearchAsync(string searchString);
    Task<ProductResponse> AddAsync(ProductRequest product);
    Task<ProductResponse> UpdateAsync(ProductRequest product);
    Task<bool> DeleteByIdAsync(Guid id);
}