using ToDoApp.Contracts;

namespace ToDoApp.Services.Abstraction;

public interface IUserService
{
    Task<UserForReadDto> RegisterNewUser(UserForCreationDto userForCreationDto,
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<UserForReadDto>> GetAllUsers(CancellationToken cancellationToken = default);
}