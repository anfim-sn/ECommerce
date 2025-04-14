using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace BusinessLogicLayer.HttpClients;

public class UsersMicroserviceClient(HttpClient httpClient, IDistributedCache cache, ILogger<UsersMicroserviceClient> logger)
{
    public async Task<UserDTO?> GetUserByUserId(Guid userId)
    {
        try
        {
            //Get cached user
            var userKey = $"user:{userId}";
            var cachedUser = await cache.GetStringAsync(userKey);

            if (cachedUser != null)
                return JsonSerializer.Deserialize<UserDTO>(cachedUser);
            
            var response = await httpClient.GetAsync($"/gateway/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return null;
                    case HttpStatusCode.BadRequest:
                        throw new HttpRequestException("Invalid request", null, HttpStatusCode.BadRequest);
                    case HttpStatusCode.ServiceUnavailable:
                        return await response.Content.ReadFromJsonAsync<UserDTO>();
                    default:
                        return new UserDTO
                        {
                            UserId = Guid.Empty,
                            Email = "Temporarly unavailable",
                            PersonName = "Temporarly unavailable",
                            Gender = "Temporarly unavailable"
                        };
                }
            }

            var user = await response.Content.ReadFromJsonAsync<UserDTO>();

            if (user == null)
                throw new HttpRequestException("User not found", null, HttpStatusCode.NotFound);

            var userToCache = JsonSerializer.Serialize(user);
            await cache.SetStringAsync(userKey, userToCache, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(3)
            });

            return user;
        }
        catch (BrokenCircuitException ex)
        {
            logger.LogError(ex, "Circuit breaker is open. Unable to call Users microservice.");
            
            return new UserDTO
            {
                UserId = Guid.Empty,
                Email = "Temporarly unavailable",
                PersonName = "Temporarly unavailable",
                Gender = "Temporarly unavailable"
            };
        }
        catch (TimeoutRejectedException ex)
        {
            logger.LogError(ex, "Request timed out. Unable to call Users microservice.");
            
            return new UserDTO
            {
                UserId = Guid.Empty,
                Email = "Temporarly unavailable",
                PersonName = "Temporarly unavailable",
                Gender = "Temporarly unavailable"
            };
        }
    }
}