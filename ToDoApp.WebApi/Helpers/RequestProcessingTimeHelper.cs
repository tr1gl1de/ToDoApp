using System.Diagnostics;
using ToDoApp.Contracts;
using ToDoApp.LoggerService;

namespace ToDoApp.WebApi.Helpers;

public static class RequestProcessingTimeHelper
{
    private static readonly ILoggerManager Logger = new LoggerManager();

    public static void RequestProcessingTime(this IApplicationBuilder app)
    {
        app.Use(async (_, next) =>
        {
            var sw = new Stopwatch();
            sw.Start();
            await next();
            sw.Stop();
            Logger.LogInfo($"Elapsed request time: {sw.Elapsed}");
        });
    }
}