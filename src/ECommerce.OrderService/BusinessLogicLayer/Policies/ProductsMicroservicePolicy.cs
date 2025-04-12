using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly;

namespace BusinessLogicLayer.Policies;

public class ProductsMicroservicePolicy(IBasePolicies basePolicies) : IProductsMicroservicePolicy
{
    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        return Policy.WrapAsync(
            basePolicies.GetFallbackPolicy<ProductDTO>(new ProductDTO
            {
                ProductId = Guid.Empty,
                ProductName = "Temporarly unavailable",
                Category = "Temporarly unavailable",
                UnitPrice = -1,
                QuantityInStock = -1
            }),
            basePolicies.GetBulkheadIsolationPolicy(2, 40)
        );
    }
}