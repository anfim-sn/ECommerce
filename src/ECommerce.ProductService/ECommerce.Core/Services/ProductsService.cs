using System.Security.Cryptography;
using AutoMapper;
using ECommerce.Core.DTO;
using ECommerce.Core.Entities;
using ECommerce.Core.RepositoryContracts;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services;

internal class ProductsService(IProductRepository productRepository, IMapper mapper) : IProductsService
{
    public async Task<List<Product>> GetListAsync()
    {
        return await productRepository.GetListAsync();
    }
    
    public async Task<Product> GetByIdAsync(int id)
    {
        return await productRepository.GetByIdAsync(id);
    }
    public async Task<List<Product>> SearchAsync(string searchString)
    {
        return await productRepository.SearchAsync(searchString);
    }
    public async Task<ProductResponse> AddAsync(ProductRequest product)
    {
        var result = await productRepository.AddAsync(mapper.Map<Product>(product));
        
        return mapper.Map<ProductResponse>(product) with{ IsSuccess = true };
    }
    
    public async Task<ProductResponse> UpdateAsync(ProductRequest product)
    {
        var result = await productRepository.UpdateAsync(mapper.Map<Product>(product));
        
        return mapper.Map<ProductResponse>(product) with{ IsSuccess = true };
    }
    public async Task<bool> DeleteByIdAsync(int id)
    {
        return await productRepository.DeleteByIdAsync(id);
    }
}