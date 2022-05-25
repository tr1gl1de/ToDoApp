using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ToDoApp.Domain.Repositories;
using ToDoApp.Persistence;
using ToDoApp.Persistence.Repositories;
using ToDoApp.Services;
using ToDoApp.Services.Abstraction;
using ToDoApp.WebApi.Middleware;

namespace ToDoApp.WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }
    
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

        services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebApi", Version = "v1"}));

        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddScoped<IRepositoryManager, RepositoryManager>();

        services.AddDbContextPool<RepositoryDbContext>(builder =>
        {
            builder.UseNpgsql(Configuration.GetConnectionString("postgresqlConnection"));
        });

        services.AddTransient<ExceptionHandlingMiddleware>();

        services.AddAutoMapper(typeof(MappingProfile).Assembly);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}