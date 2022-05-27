using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NLog;
using ToDoApp.Contracts;
using ToDoApp.Entities;
using ToDoApp.LoggerService;
using ToDoApp.Repository;

namespace ToDoApp.WebApi;

internal class Startup
{
    private readonly IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/NLog.config"));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var projectDirectory = AppContext.BaseDirectory;
            var projectName = Assembly.GetExecutingAssembly().GetName().Name;
            var xmlFileName = $"{projectName}.xml";
            
            options.IncludeXmlComments(Path.Combine(projectDirectory,xmlFileName));
        });
        services.AddDbContext<RepositoryDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddSingleton<ILoggerManager, LoggerManager>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}