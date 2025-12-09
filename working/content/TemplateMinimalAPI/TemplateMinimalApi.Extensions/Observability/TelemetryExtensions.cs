namespace TemplateMinimalApi.Extensions.Observability;

public static class TelemetryExtensions
{
    public static IServiceCollection AddApplicationInsightsApiTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationInsights");

        var options = new ApplicationInsightsServiceOptions
        {
            EnableAdaptiveSampling = true,
            ConnectionString = connectionString,
            EnableHeartbeat = false
        };

        services.AddApplicationInsightsTelemetry(options);

        return services;
    }

    public static IServiceCollection AddApplicationInsightsWorkerTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationInsights");

        services.AddApplicationInsightsTelemetryWorkerService(new Microsoft.ApplicationInsights.WorkerService.ApplicationInsightsServiceOptions
        {
            ConnectionString = connectionString
        });

        return services;
    }

    public static IServiceCollection AddOpenTelemetryExtension(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
       .WithTracing(tracing =>
       {
           tracing.AddAspNetCoreInstrumentation();
           tracing.AddHttpClientInstrumentation();

           var endpoint = configuration["OpenTelemetry:OtlpEndpoint"];

           if (!string.IsNullOrWhiteSpace(endpoint))
           {
               tracing.AddOtlpExporter(options => options.Endpoint = new Uri(endpoint));
           }
       })
       .WithMetrics(metrics =>
       {
           metrics.AddAspNetCoreInstrumentation();
           metrics.AddHttpClientInstrumentation();

           var endpoint = configuration["OpenTelemetry:OtlpEndpoint"];

           if (!string.IsNullOrWhiteSpace(endpoint))
           {
               metrics.AddOtlpExporter(options => options.Endpoint = new Uri(endpoint));
           }
       });

        return services;
    }
}
