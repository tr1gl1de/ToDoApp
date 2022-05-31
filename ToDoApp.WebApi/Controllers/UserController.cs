using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Contracts;
using ToDoApp.Entities.DataTransferObjects.User;
using ToDoApp.Entities.Models;

namespace ToDoApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : ControllerBase
{
    private ILoggerManager _logger;
    private IRepositoryWrapper _repository;
    private IMapper _mapper;

    public UserController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>Register a new user.</summary>
    /// <param name="userRegDto">User registration information.</param>
    /// <response code="200">Newly registered user.</response>
    /// <response code="409">Failed to register a user: username already taken.</response>
    [ProducesResponseType(typeof(UserForReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegisterDto userRegDto)
    {
        var userIsExist = await _repository.User.UserIsExits(userRegDto.Username);
        if (userIsExist)
        {
            return Conflict("Username already taken.");
        }

        var newUser = _mapper.Map<User>(userRegDto);
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            
        _repository.User.CreateUser(newUser);
        await _repository.SaveAsync();
            
        var userRead = _mapper.Map<UserForReadDto>(newUser);
            
        _logger.LogInfo($"User with id: {userRead.Id} and username : {userRead.Username}");
            
        return Ok(userRead);
    }
}