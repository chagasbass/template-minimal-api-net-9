namespace TemplateMinimalApi.Extensions.Observability.Healthchecks;

public static class HealthcheckExtensions
{
    /// <summary>
    /// Efetua a configuração dos healthchecks customizados e da UI da dashboard que será usada
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        #region healthchecks customizados
        services.AddHealthChecks()
                .AddGCInfoCheck(HealthNames.MemoryHealthcheck, default, HealthNames.MemoryTags)
                .AddSelfCheck(HealthNames.SelfHealthcheck, default, HealthNames.SelfTags);

        #endregion

        return services;
    }



    public static IApplicationBuilder UseHealthChecksMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHealthChecks(configuration);

        return app;
    }

    /// <summary>
    /// Extensão que para customizar o as informações dos healthchecks e retornar um json customizado
    /// em um determinado endpoint
    /// </summary>
    /// <param name="app"></param>
    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHealthChecks("/healthz");
        app.UseHealthChecks("/healthz-json",
             new HealthCheckOptions()
             {
                 ResponseWriter = async (context, report) =>
                 {
                     string result = report.AddHealthStatusData(configuration);

                     context.Response.ContentType = MediaTypeNames.Application.Json;

                     await context.Response.WriteAsync(result);
                 }
             });

        return app;
    }
}
