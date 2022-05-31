using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>Authenticate the user.</summary>
    /// <param name="userForAuth">User authentication information.</param>
    /// <response code="200">Authentication token pair for specified user credentials.</response>
    /// <response code="404">User with specified username does not exist.</response>
    /// <response code="409">Incorrect password was specified.</response>
    [HttpPost("authenticate")]
    [ProducesResponseType(typeof(TokenPairDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        _repository.RefreshToken.AddRefreshToken(refreshToken);
        await _repository.SaveAsync();

        var tokenPair = _tokenHelper.IssueTokenPair(user.Id, refreshToken.Id);
        var tokenPairDto = _mapper.Map<TokenPairDto>(tokenPair);

        return Ok(tokenPairDto);
    }

    /// <summary>Refresh token pair.</summary>
    /// <param name="refreshToken">Refresh token.</param>
    /// <response code="200">A new token pair.</response>
    /// <response code="400">Invalid refresh token was provided.</response>
    /// <response code="409">Provided refresh token has already been used.</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(TokenPairDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RefreshTokenPair([FromBody] string refreshToken)
    {
        var refreshTokenClaims = _tokenHelper.ParseToken(refreshToken);
        if (refreshTokenClaims is null)
        {
            return BadRequest("Invalid refresh token was provided.");
        }

        var refreshTokenId = Guid.Parse(refreshTokenClaims["jti"]);
        var refreshTokenEntity = await _repository.RefreshToken.GetRefreshTokenByIdAsync(refreshTokenId);
        if (refreshTokenEntity is null)
        {
            return Conflict("Provided refresh token has already been used");
        }
        
        _repository.RefreshToken.DeleteRefreshToken(refreshTokenEntity);
        await _repository.SaveAsync();

        var userId = Guid.Parse(refreshTokenClaims["sub"]);
        var refreshTokenLifetime = int.Parse(_configuration["JwtAuth:RefreshTokenLifetime"]);
        var newRefreshTokenEntity = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ExpirationTime = DateTime.UtcNow.AddDays(refreshTokenLifetime)
        };
        
        _repository.RefreshToken.AddRefreshToken(newRefreshTokenEntity);
        await _repository.SaveAsync();

        var tokenPair = _tokenHelper.IssueTokenPair(userId, refreshTokenEntity.Id);
        var tokenPairDto = _mapper.Map<TokenPairDto>(tokenPair);

        return Ok(tokenPairDto);
    }

    /// <summary>Get information about the current user.</summary>
    /// <response code="200">Current user information.</response>
    [HttpGet("who-am-i")]
    [Authorize]
    [ProducesResponseType(typeof(UserForReadDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var userId = Guid.Parse(subClaim.Value);

        var user = await _repository.User.GetUserById(userId);

        var userDto = _mapper.Map<UserForReadDto>(user);

        return Ok(userDto);
    }
}