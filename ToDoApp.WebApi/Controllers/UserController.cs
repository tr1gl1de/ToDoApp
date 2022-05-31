using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Contracts;
using ToDoApp.Entities.DataTransferObjects;
using ToDoApp.Entities.DataTransferObjects.User;
using ToDoApp.Entities.Models;
using ToDoApp.WebApi.Helpers;

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
    private IConfiguration _configuration;
    private JwtTokenHelper _tokenHelper;

    public UserController(
        ILoggerManager logger,
        IRepositoryWrapper repository,
        IMapper mapper,
        IConfiguration configuration,
        JwtTokenHelper tokenHelper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
        _tokenHelper = tokenHelper;
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

    [HttpPost("auth")]
    public async Task<IActionResult> AuthenticateUser([FromBody] UserForAuthenticateDto userForAuth)
    {
        var user = await _repository.User
            .GetUserByUsername(userForAuth.Username);
        if (user is null)
        {
            return NotFound("Not found user with this username");
        }

        if (!BCrypt.Net.BCrypt.Verify(userForAuth.Password, user.Password))
        {
            return Conflict("Incorrect password");
        }

        var refreshTokenLifetime = int.Parse(_configuration["JwtAuth:RefreshTokenLifetime"]);
        var refreshToken = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            ExpirationTime = DateTime.UtcNow.AddDays(refreshTokenLifetime)
        };
        _repository.AddRefreshToken(refreshToken);
        await _repository.SaveAsync();

        var tokenPair = _tokenHelper.IssueTokenPair(user.Id, refreshToken.Id);
        var tokenPairDto = _mapper.Map<TokenPairDto>(tokenPair);

        return Ok(tokenPairDto);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid userId)
    {
        var user = await _repository.User.GetUserById(userId);
        return Ok(user);
    }
    
    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetUserByUsername([FromRoute] string username)
    {
        var user = await _repository.User.GetUserByUsername(username);
        return Ok(user);
    }

    [HttpGet("username/{username}/details")]
    public async Task<IActionResult> GetUSerByUsernameWithNotes([FromRoute] string username)
    {
        var user = await _repository.User.GetUserByUsernameWithNotes(username);
        return Ok(user);
    }
}