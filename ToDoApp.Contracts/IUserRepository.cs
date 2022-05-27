using ToDoApp.Entities.Models;

namespace ToDoApp.Contracts;

public interface IUserRepository
{
    Task<User> GetUserById(Guid id);
    Task<User> GetUserByUsername(string username);
    void CreateUser(User user);
}