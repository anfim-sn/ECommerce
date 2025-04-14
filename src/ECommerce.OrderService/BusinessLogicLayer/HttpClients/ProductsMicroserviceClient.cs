using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;

namespace BusinessLogicLayer.HttpClients;

public class ProductsMicroserviceClient(HttpClient httpClient, IDistributedCache cache, ILogger<ProductsMicroserviceClient> logger)
{
    public async Task<ProductDTO?> GetProductByProductId(Guid productId)
    {
        try
        {
            //Get cached product
            var productKey = $"product:{productId}";
            var cachedProduct = await cache.GetStringAsync(productKey);
            
            if (cachedProduct != null)
                return JsonSerializer.Deserialize<ProductDTO>(cachedProduct);
            
            var response = await httpClient.GetAsync($"/api/products/search/productid/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return null;
                    case HttpStatusCode.BadRequest:
                        throw new HttpRequestException("Invalid request", null, HttpStatusCode.BadRequest);
                    case HttpStatusCode.ServiceUnavailable:
                        return await response.Content.ReadFromJsonAsync<ProductDTO>();
                    default:
                        throw new HttpRequestException("Http request failure with status code", null, response.StatusCode);
                }
            }
            
            var product = await response.Content.ReadFromJsonAsync<ProductDTO>();
            
            if (product == null)
                throw new HttpRequestException("Product not found", null, HttpStatusCode.NotFound);
            
            var productToCache = JsonSerializer.Serialize(product);
            await cache.SetStringAsync(productKey, productToCache, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                SlidingExpiration = TimeSpan.FromSeconds(100)
            });
            
            return product;
        }
        catch (BulkheadRejectedException e)
        {
            logger.LogError(e, "Bulkhead rejected the request to Products Microservice");
            return new ProductDTO
            {
                ProductId = Guid.Empty,
                ProductName = "Temporarly Unavailable",
                Category = "Temporarly Unavailable",
                UnitPrice = -1,
                QuantityInStock = -1
            };
        }
    }
}