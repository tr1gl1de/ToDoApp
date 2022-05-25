using AutoMapper;
using ToDoApp.Contracts;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Exceptions;
using ToDoApp.Domain.Repositories;
using ToDoApp.Services.Abstraction;

namespace ToDoApp.Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public UserService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserForReadDto>> GetAllUsers(CancellationToken cancellationToken = default)
    {
        var users = await _repositoryManager.UserRepository.GetAllAsync(cancellationToken);
        var usersDto = _mapper.Map<IEnumerable<UserForReadDto>>(users);
        return usersDto;
    }

    public async Task<UserForReadDto> RegisterNewUser(UserForCreationDto userForCreationDto, CancellationToken cancellationToken = default)
    {
        var usernameTaken = await _repositoryManager.UserRepository
            .UsernameIsExist(userForCreationDto.Username, cancellationToken);
        if (usernameTaken)
        {
            throw new UserConflictException();
        }

        var newUser = _mapper.Map<User>(userForCreationDto);
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(userForCreationDto.Password);
        newUser.DateCreation = DateTime.UtcNow;

        _repositoryManager.UserRepository.Insert(newUser);
        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

        var readUser = _mapper.Map<UserForReadDto>(newUser);

        return readUser;
    }
}