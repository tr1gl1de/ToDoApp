using Microsoft.EntityFrameworkCore;
using ToDoApp.Persistence;

namespace ToDoApp.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var webHost = CreateHostBuilder(args).Build();
        await ApplyMigrations(webHost.Services);
        await webHost.RunAsync();
    }

    private static async Task ApplyMigrations(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        await using RepositoryDbContext dbContext = scope.ServiceProvider.GetRequiredService<RepositoryDbContext>();

        // for real db
        await dbContext.Database.MigrateAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>());
}