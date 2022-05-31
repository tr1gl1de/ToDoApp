﻿using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoApp.Entities;
using ToDoApp.WebApi.Helpers;

namespace ToDoApp.WebApi.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services) =>
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

    public static SymmetricSecurityKey GetAuthSecret(this IConfiguration configuration)
    {
        var secret = configuration["JwtAuth:Secret"];
        var secretBytes = Encoding.ASCII.GetBytes(secret);
        return new SymmetricSecurityKey(secretBytes);
    }

    public static TimeSpan GetAccessTokenLifetime(this IConfiguration configuration)
    {
        var accessTokenLifetimeInMinutes = int.Parse(configuration["JwtAuth:AccessTokenLifetime"]);
        return TimeSpan.FromMinutes(accessTokenLifetimeInMinutes);
    }

    public static TimeSpan GetRefreshTokenLifetime(this IConfiguration configuration)
    {
        var refreshTokenLifetimeInDays = int.Parse(configuration["JwtAuth:RefreshTokenLifetime"]);
        return TimeSpan.FromMinutes(refreshTokenLifetimeInDays);
    }
}