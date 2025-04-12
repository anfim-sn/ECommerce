using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace BusinessLogicLayer.HttpClients;

public class UsersMicroserviceClient(HttpClient httpClient, ILogger<UsersMicroserviceClient> logger)
{
    public async Task<UserDTO?> GetUserByUserId(Guid userId)
    {
        try
        {
            var response = await httpClient.GetAsync($"/api/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return null;
                    case HttpStatusCode.BadRequest:
                        throw new HttpRequestException("Invalid request", null, HttpStatusCode.BadRequest);
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
            
            return await response.Content.ReadFromJsonAsync<UserDTO>();
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