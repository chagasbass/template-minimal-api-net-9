namespace TemplateMinimalApi.Extensions.Extensions.Observability.HealthChecks;

/// <summary>
/// Extension para HealthCheck do GarbageCollector
/// </summary>
public static class GCInfoHealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddGCInfoCheck(
        this IHealthChecksBuilder builder,
        string name,
        HealthStatus? failureStatus = null,
        IEnumerable<string> tags = null,
        long? thresholdInBytes = null)
    {
        builder.AddCheck<GarbageCollectorHealthcheck>(name, failureStatus ?? HealthStatus.Degraded, tags);

        if (thresholdInBytes.HasValue)
        {
            builder.Services.Configure<GCInfoOptions>(name, options =>
            {
                options.Threshold = thresholdInBytes.Value;
            });
        }

        return builder;
    }
}
