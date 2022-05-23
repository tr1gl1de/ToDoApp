using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

    Task<bool> UsernameIsExist(string username, CancellationToken cancellationToken = default);

    void Insert(User user);
}