namespace TemplateMinimalApi.Extensions.CustomLogs;

public static class LogIntegrationsExtensions
{
    public static IServiceCollection AddFilterToSystemLogs(this IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddFilter("Microsoft", LogLevel.Warning)
                   .AddFilter("System", LogLevel.Warning)
                   .AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning)
                   .AddFilter("Microsoft.AspNetCore", LogLevel.Warning)
                   .AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning)
                   .AddFilter("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogLevel.None)
                   .AddFilter("Microsoft.AspNetCore.OpenApi", LogLevel.None)
                   .AddFilter("TinyHealthCheck", LogLevel.Warning)
                   .AddConsole();
        });

        return services;
    }

    public static IServiceCollection AddLogServiceDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ILogServices, LogServices>();
        services.AddSingleton<LogData>();

        return services;
    }

    public static IServiceCollection AddNotificationControl(this IServiceCollection services)
    {
        services.AddSingleton<INotificationServices, NotificationServices>();

        return services;
    }
}
