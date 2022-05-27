using Microsoft.EntityFrameworkCore;
using ToDoApp.Contracts;
using ToDoApp.Entities;
using ToDoApp.Entities.Models;

namespace ToDoApp.Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryDbContext repositoryDbContext) : base(repositoryDbContext)
    {
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var user = await FindByCondition(u => u.Id == id)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var user = await FindByCondition(u => u.Username == username)
            .FirstOrDefaultAsync();
        return user;
    }

    public void CreateUser(User user)
    {
        Create(user);
    }
}