using Polly;

namespace BusinessLogicLayer.Policies;

public interface IUsersMicroservicePolicy
{
    IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
    
}