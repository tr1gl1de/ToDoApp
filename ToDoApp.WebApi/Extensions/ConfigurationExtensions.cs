﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ToDoApp.WebApi.Extensions;

public static class ConfigurationExtensions
{

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