using System.Net;
using System.Text;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;

namespace BusinessLogicLayer.Policies;

public class BasePolicies(ILogger<BasePolicies> logger) : IBasePolicies
{
    public IAsyncPolicy<HttpResponseMessage> GetFallbackPolicy<TOut>(TOut dataToReturn)
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .FallbackAsync(async (context) => {
                logger.LogInformation($"Fallback triggered, returning default product data");
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(dataToReturn), Encoding.UTF8, "application/json")
                };
            });
    }

    public IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicy(int maxParallelization, int maxQueuingActions)
    {
        return Policy.BulkheadAsync<HttpResponseMessage>(
            maxParallelization,
            maxQueuingActions,
            onBulkheadRejectedAsync: (context) => {
                logger.LogInformation("Bulkhead queue is full, rejecting request.");
                throw new BulkheadRejectedException();
            });
    }

    public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
    {
        return Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).WaitAndRetryAsync(
            retryCount,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryAttempt, context) => {
                logger.LogInformation($"Retrying due to {outcome.Result.StatusCode} at {DateTime.UtcNow} for {retryAttempt} time");
            });
    }

    public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak)
    {
        return Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking,
                durationOfBreak,
                onBreak: (outcome, timespan) => {
                    logger.LogWarning($"Circuit broken due to {outcome.Result.StatusCode} at {DateTime.UtcNow}");
                },
                onReset: () => {
                    logger.LogInformation($"Circuit reset at {DateTime.UtcNow}");
                });
    }

    public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeout)
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(timeout);
    }
}