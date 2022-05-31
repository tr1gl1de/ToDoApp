using ToDoApp.Entities.Models;

namespace ToDoApp.Contracts;

public interface IUserRepository
{
    Task<User?> GetUserById(Guid id);
    Task<User?> GetUserByUsername(string username);
    Task<bool> UserIsExits(string username);
    Task<bool> UserIsExits(Guid id);
    void CreateUser(User user);
}