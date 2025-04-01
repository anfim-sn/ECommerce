using ECommerce.Core.DTO;

namespace ECommerce.Core.ServiceContracts;

public interface IUsersService
{
    /// <summary>
    /// Login a user
    /// </summary>
    Task<AuthenticationResponse?> Login(LoginRequest loginRequest);
    
    /// <summary>
    /// Register user
    /// </summary>
    Task<AuthenticationResponse?> Register(RegisterRequest loginRequest);
    
    /// <summary>
    /// Get user by UserId
    /// </summary>
    Task<UserDTO?> GetUserByUserId(Guid userId);
}