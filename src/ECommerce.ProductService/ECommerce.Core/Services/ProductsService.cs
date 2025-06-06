using AutoMapper;
using ECommerce.Core.DTO;
using ECommerce.Core.Entities;
using ECommerce.Core.RabbitMQ;
using ECommerce.Core.RepositoryContracts;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services;

internal class ProductsService(IProductRepository productRepository, IRabbitMQPublisher rabbitMQPublisher, IMapper mapper) : IProductsService
{
    public async Task<List<Product>> GetListAsync()
    {
        return await productRepository.GetListAsync();
    }
    
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await productRepository.GetByIdAsync(id);
    }
    public async Task<List<Product>> SearchAsync(string searchString)
    {
        return await productRepository.SearchAsync(searchString);
    }
    public async Task<ProductResponse> AddAsync(ProductRequest product)
    {
        var isSuccess = false;
        var affectedRow = await productRepository.AddAsync(mapper.Map<Product>(product));
        
        if (affectedRow > 0)
            isSuccess = true;
        
        return mapper.Map<ProductResponse>(product) with { IsSuccess = isSuccess };
    }
    
    public async Task<ProductResponse> UpdateAsync(ProductRequest productRequest)
    {
        var existingProduct = productRepository.GetByCondition(temp => temp.ProductId == productRequest.ProductId, false);

        if (existingProduct == null)
            throw new ArgumentException("Invalid Product ID");
        
        var isSuccess = false;

        var product = mapper.Map<Product>(productRequest);
        
        var affectedRow = await productRepository.UpdateAsync(product);

        if (affectedRow > 0)
            isSuccess = true;
        
        var headers = new Dictionary<string, object>()
        {
            {"x-match", "all"},
            {"event", "product.update"},
            {"RowCount", 1},
        };
        
        rabbitMQPublisher.Publish<Product>(headers, product);
        
        return mapper.Map<ProductResponse>(productRequest) with { IsSuccess = isSuccess };
    }
    
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var isSuccess =  await productRepository.DeleteByIdAsync(id);

        if (isSuccess)
        {
            var headers = new Dictionary<string, object>()
            {
                {"event", "product.delete"},
                {"RowCount", 1}
            };
            var message = new ProductDeleteMessage(id);

            rabbitMQPublisher.Publish<ProductDeleteMessage>(headers, message);
        }
        
        return isSuccess;
    }
}