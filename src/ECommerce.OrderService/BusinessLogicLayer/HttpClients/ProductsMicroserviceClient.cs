using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;

namespace BusinessLogicLayer.HttpClients;

public class ProductsMicroserviceClient(HttpClient httpClient, ILogger<ProductsMicroserviceClient> logger)
{
    public async Task<ProductDTO?> GetProductByProductId(Guid productId)
    {
        try
        {
            var response = await httpClient.GetAsync($"/api/products/search/productid/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return null;
                    case HttpStatusCode.BadRequest:
                        throw new HttpRequestException("Invalid request", null, HttpStatusCode.BadRequest);
                    default:
                        throw new HttpRequestException("Http request failure with status code", null, response.StatusCode);
                }
            }
            
            return await response.Content.ReadFromJsonAsync<ProductDTO>();
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