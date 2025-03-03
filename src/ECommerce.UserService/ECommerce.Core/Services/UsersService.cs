using ECommerce.Core.DTO;
using ECommerce.Core.Entities;
using ECommerce.Core.RepositoryContracts;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services;

internal class UsersService(IUserRepository userRepository) : IUsersService
{
    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        var user = await userRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

        if (user == null)
            return null;
        
        return new AuthenticationResponse(
            user.UserId, 
            user.Email, 
            user.PersonName, 
            user.Gender, 
            Token: "token", 
            Success: true);
    }
    
    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        var user = await userRepository.AddUser(new ApplicationUser
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                PersonName = registerRequest.PersonName,
                Gender = registerRequest.Gender.ToString()
            }
        );

        if (user == null)
            return null;

        return new AuthenticationResponse(
            user.UserId, 
            user.Email, 
            user.PersonName, 
            user.Gender, 
            Token: "token", 
            Success: true);
    }
}