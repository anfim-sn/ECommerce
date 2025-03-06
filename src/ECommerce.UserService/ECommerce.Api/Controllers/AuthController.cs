using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")] //api/auth
[ApiController]
public class AuthController(IUsersService usersService) : ControllerBase
{

    [HttpPost("login")]//POST api/auth/login
    public async Task<IActionResult> Login(LoginRequest? loginRequest)
    {
        if (loginRequest == null)
            return BadRequest("Invalid login data");

        var authenticationResponse = await usersService.Login(loginRequest);

        if (authenticationResponse == null || !authenticationResponse.Success)
            return Unauthorized(authenticationResponse);

        return Ok(authenticationResponse);
    }
    
    [HttpPost("register")] //POST api/auth/register
    public async Task<IActionResult> Register(RegisterRequest? registerRequest)
    {
        if (registerRequest == null)
            return BadRequest("Invalid registration data");
        
        var authenticationResponse = await usersService.Register(registerRequest);
        
        if (authenticationResponse == null || !authenticationResponse.Success)
            return BadRequest(authenticationResponse);

        return Ok(authenticationResponse);
    }
}