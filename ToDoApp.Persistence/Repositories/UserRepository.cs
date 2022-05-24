using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RepositoryDbContext _context;

    public UserRepository(RepositoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);
        return users;
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return user;
    }

    public async Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        return user;
    }

    public async Task<bool> UsernameIsExist(string username, CancellationToken cancellationToken = default)
    {
        var userIsExist = await _context.Users
            .AnyAsync(x => x.Username == username, cancellationToken);
        return userIsExist;
    }

    public void Insert(User user)
    {
        _context.Users.Add(user);
    }
}