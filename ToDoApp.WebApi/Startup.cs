using System.Reflection;
using NLog;
using ToDoApp.WebApi.Extensions;
using ToDoApp.WebApi.Helpers;

namespace ToDoApp.WebApi;

internal class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/NLog.config"));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwagger();
        services.AddDbContext();
        services.AddJwtAuth(_configuration);
        services.AddCorsPolicy();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.ConfigureExceptionHandler();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseCors("EnableCORS");
        app.UseAuthentication();
        app.UseAuthorization();
        app.RequestProcessingTime();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}