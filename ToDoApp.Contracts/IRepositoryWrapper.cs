using ToDoApp.Entities.Models;

namespace ToDoApp.Contracts;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }
    INoteRepository Note { get; }
    IRefreshTokenRepository RefreshToken { get; }
    Task SaveAsync();
}