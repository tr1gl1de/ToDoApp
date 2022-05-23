using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Persistence;

public class RepositoryDbContext : DbContext
{
    public RepositoryDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Note> Notes { get; set; }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) =>
        builder.ApplyConfigurationsFromAssembly(typeof(RepositoryDbContext).Assembly);
}