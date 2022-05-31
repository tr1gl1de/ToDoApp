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
            var currentAssembly = Assembly.GetExecutingAssembly();  
            var xmlDocs = currentAssembly.GetReferencedAssemblies()  
                .Union(new AssemblyName[] { currentAssembly.GetName()})  
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))  
                .Where(f=>File.Exists(f)).ToArray(); 
            
            Array.ForEach(xmlDocs, (d) =>  
            {  
                options.IncludeXmlComments(d);  
            });  
            
        });
        services.AddDbContext<RepositoryDbContext>(options =>
            options.UseInMemoryDatabase("test_base"));
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