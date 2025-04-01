using ECommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]//api/auth
[ApiController]
public class UsersController(IUsersService usersService) : ControllerBase
{
    [HttpGet("{userId}")] //GET api/users/{userId}
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            return BadRequest("Invalid user id");
        
        var user = await usersService.GetUserByUserId(userId);
        
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }
}