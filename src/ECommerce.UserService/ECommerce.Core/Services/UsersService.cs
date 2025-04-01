using System.Buffers.Text;
using System.Runtime.Intrinsics.Arm;
using AutoMapper;
using ECommerce.Core.DTO;
using ECommerce.Core.Entities;
using ECommerce.Core.RepositoryContracts;
using ECommerce.Core.ServiceContracts;

namespace ECommerce.Core.Services;

internal class UsersService(IUserRepository userRepository, IMapper mapper) : IUsersService
{
    public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
    {
        var user = await userRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

        if (user == null)
            return null;

        return mapper.Map<AuthenticationResponse>(user) 
            with { Success = true, Token = "token" };
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
    {
        var user = await userRepository.AddUser(mapper.Map<ApplicationUser>(registerRequest));

        if (user == null)
            return null;

        return mapper.Map<AuthenticationResponse>(user)
            with { Success = true, Token = "token" };
    }
    public async Task<UserDTO?> GetUserByUserId(Guid userId)
    {
        var userApplication = await userRepository.GetUserByUserId(userId);
        
        return mapper.Map<UserDTO>(userApplication);
    }
}