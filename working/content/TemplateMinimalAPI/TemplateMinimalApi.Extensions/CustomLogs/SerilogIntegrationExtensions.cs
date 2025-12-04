namespace TemplateMinimalApi.Extensions.CustomLogs;

public static class LogIntegrationsExtensions
{
    public static Logger ConfigureStructuralLogWithSerilog(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationInsights");

        var telemetryConfiguration = new TelemetryConfiguration
        {
            ConnectionString = connectionString
        };

        if (!string.IsNullOrEmpty(connectionString))
        {
            return new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
        .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware", LogEventLevel.Fatal)
        .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck-ui")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksDb")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksUI")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("healthchecks-data-ui")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger/index.html")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger/index.html")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger/v1/swagger.json")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger/v1/swagger.json")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("healthz-json")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthz-json")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().StartsWith("SourceContext, 'Microsoft.AspNetCore.Diagnostics'")))
        .Destructure.ByTransforming<HttpRequest>(x => new
        {
            x.Method,
            Url = x.Path,
            x.QueryString
        })
        .WriteTo.Console()
        .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
        .CreateLogger();
        }

        return new LoggerConfiguration()
       .MinimumLevel.Information()
       .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
        .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware", LogEventLevel.Fatal)
        .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning)
       .Enrich.FromLogContext()
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck-ui")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksDb")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksUI")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("healthchecks-data-ui")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger/index.html")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger/index.html")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger/v1/swagger.json")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger/v1/swagger.json")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("healthz-json")))
       .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthz-json")))
       .Destructure.ByTransforming<HttpRequest>(x => new
       {
           x.Method,
           Url = x.Path,
           x.QueryString
       })
       .WriteTo.Console()
       .CreateLogger();
    }

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
