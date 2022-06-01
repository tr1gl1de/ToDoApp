using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Entities.Extensions;
using ToDoApp.Entities.Models;

namespace ToDoApp.Entities;

public class RepositoryDbContext : DbContext
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