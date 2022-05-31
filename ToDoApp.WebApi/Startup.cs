using System.Reflection;
using NLog;
using ToDoApp.Contracts;
using ToDoApp.LoggerService;
using ToDoApp.Repository;
using ToDoApp.WebApi.Extensions;

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
        services.AddSwagger();
        services.AddDbContext();
        services.AddJwtAuth(Configuration);
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