using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDo.Persistence;
using ToDoApp.Contracts;
using ToDoApp.Entities.Helpers;
using ToDoApp.Entities.Models;
using ToDoApp.LoggerService;
using ToDoApp.Repository;
using ToDoApp.WebApi.Filters;
using ToDoApp.WebApi.Helpers;

namespace ToDoApp.WebApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen(options =>
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var xmlDocs = currentAssembly.GetReferencedAssemblies()
                .Union(new AssemblyName[] {currentAssembly.GetName()})
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
                .Where(f => File.Exists(f)).ToArray();

            Array.ForEach(xmlDocs, (d) =>
            {
                options.IncludeXmlComments(d);
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Put Your access token here (drop **Bearer** prefix):",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.OperationFilter<OpenApiFilter>();

        });

    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<JwtTokenHelper>()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtSecret = Encoding.ASCII.GetBytes(configuration["JwtAuth:Secret"]);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),

                    ValidateAudience = false,
                    ValidateIssuer = false,

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.RequireHttpsMetadata = false;

                var tokenHandler = options
                    .SecurityTokenValidators
                    .OfType<JwtSecurityTokenHandler>()
                    .Single();
                tokenHandler.InboundClaimTypeMap.Clear();
                tokenHandler.OutboundClaimTypeMap.Clear();
            });
        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<RepositoryDbContext>(options =>
            options.UseInMemoryDatabase("test_base"));
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISortHelper<Note>, SortHelper<Note>>();
        services.AddSingleton<ILoggerManager, LoggerManager>();
        services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddPolicy("EnableCORS", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));
        return services;
    }
}