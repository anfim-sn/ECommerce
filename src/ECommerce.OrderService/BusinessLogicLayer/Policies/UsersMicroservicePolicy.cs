using Microsoft.Extensions.Logging;
using Polly;

namespace BusinessLogicLayer.Policies;

public class UsersMicroservicePolicy(IBasePolicies basePolicies) : IUsersMicroservicePolicy
{
    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        return Policy.WrapAsync(
            basePolicies.GetRetryPolicy(5),
            basePolicies.GetCircuitBreakerPolicy(3, TimeSpan.FromMinutes(2)),
            basePolicies.GetTimeoutPolicy(TimeSpan.FromMilliseconds(1500)));
    }
}