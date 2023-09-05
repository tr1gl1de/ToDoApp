using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using ToDoApp.Contracts;
using ToDoApp.Entities.Models;
using ToDoApp.LoggerService;

namespace ToDoApp.WebApi.Extensions;

public static class ServiceExtensions
{
    private static readonly ILoggerManager Logger = new LoggerManager();
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    Logger.LogError($"Something went wrong in {contextFeature.Endpoint?.DisplayName}: " +
                                    $"{contextFeature.Error.Message}");
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server error."
                    }.ToString());
                }
            }));
    }
}