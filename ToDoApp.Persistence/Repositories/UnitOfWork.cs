using ToDoApp.Domain.Repositories;

namespace ToDoApp.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RepositoryDbContext _context;

    public UnitOfWork(RepositoryDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}