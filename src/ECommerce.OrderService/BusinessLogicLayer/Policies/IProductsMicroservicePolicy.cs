using Polly;

namespace BusinessLogicLayer.Policies;

public interface IProductsMicroservicePolicy
{
    IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
}