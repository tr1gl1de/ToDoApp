using Microsoft.AspNetCore.Mvc;
using ToDoApp.Contracts;
using ToDoApp.Services.Abstraction;

namespace Presentation.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public UserController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] UserForCreationDto userDto,
        CancellationToken cancellationToken)
    {
        var newUser = await _serviceManager.UserService
            .RegisterNewUser(userDto, cancellationToken);
        return Ok(newUser);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _serviceManager.UserService.GetAllUsers(cancellationToken);
        return Ok(users);
    }
}