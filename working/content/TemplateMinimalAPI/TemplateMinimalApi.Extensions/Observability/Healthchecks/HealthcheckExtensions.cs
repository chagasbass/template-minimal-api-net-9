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

    public static IServiceCollection AddMapHealthChecksUi(this IServiceCollection services, IConfiguration configuration)
    {

        var healthcheckOptions = configuration.GetSection(HealthchecksConfigurationOptions.HealthConfig).Get<HealthchecksConfigurationOptions>();

        #region healthcheckUI
        services.AddHealthChecksUI(setupSettings: setup =>
        {
            setup.SetHeaderText(healthcheckOptions.NomeAplicacao);
            setup.SetEvaluationTimeInSeconds(healthcheckOptions.TempoDePooling);
            setup.MaximumHistoryEntriesPerEndpoint(healthcheckOptions.MaximoDeEntradaPorEndpoints);
        }).AddInMemoryStorage();
        #endregion


        //var nomeAplicacao = configuration["HealthchecksConfiguration:NomeAplicacao"];
        //var tempoPooling = Int32.Parse(configuration["HealthchecksConfiguration:TempoDePooling"]);
        //var maximoDeEntradaPorEndpoints = Int32.Parse(configuration["HealthchecksConfiguration:MaximoDeEntradaPorEndpoints"]);

        //#region healthcheckUI
        //services.AddHealthChecksUI(setupSettings: setup =>
        //{
        //    setup.SetHeaderText(nomeAplicacao);
        //    setup.SetEvaluationTimeInSeconds(tempoPooling);
        //    setup.MaximumHistoryEntriesPerEndpoint(maximoDeEntradaPorEndpoints);
        //}).AddInMemoryStorage();
        //#endregion

        return services;
    }

    public static IApplicationBuilder UseHealthChecksMiddleware(this IApplicationBuilder app, IConfiguration configuration, bool hasUI = false)
    {
        app.UseHealthChecks(configuration);

        if (hasUI)
            app.UserHealthCheckUi();

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

    /// <summary>
    /// Configuração do middlelware do healthcheck UI
    /// </summary>
    /// <param name="app"></param>
    public static IApplicationBuilder UserHealthCheckUi(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/healthchecks-data-ui", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Ativa o dashboard para a visualização da situação de cada Health Check
        app.UseHealthChecksUI(options =>
        {
            options.UIPath = "/monitor";
            //options.AddCustomStylesheet("dotnet.css"); add customização na dashboard
        });

        return app;
    }
}
