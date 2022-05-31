using ToDoApp.Entities.Models;

namespace ToDoApp.Contracts;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }
    INoteRepository Note { get; }
    void AddRefreshToken(RefreshToken token);
    Task SaveAsync();
}