using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDo.Persistence.Extensions;
using ToDoApp.Entities.Models;

namespace ToDo.Persistence;

public sealed class RepositoryDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder) =>
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
            .Seed();
}