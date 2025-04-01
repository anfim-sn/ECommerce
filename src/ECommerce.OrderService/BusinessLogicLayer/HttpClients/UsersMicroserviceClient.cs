using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.HttpClients;

public class UsersMicroserviceClient(HttpClient httpClient)
{
    public async Task<UserDTO?> GetUserByUserId(Guid userId)
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
                    throw new HttpRequestException("Http request failure with status code", null, response.StatusCode);
            }
        }
        
        return await response.Content.ReadFromJsonAsync<UserDTO>();
    }
}