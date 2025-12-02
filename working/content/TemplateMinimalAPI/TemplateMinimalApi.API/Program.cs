using TemplateMinimalApi.Extensions.Authentications;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger, dispose: true);

try
{
    #region configuracoes das extensoes

    builder.Services.AddEndpointsApiExplorer()
                    .AddProblemDetails()
                    .AddBaseConfigurationOptionsPattern(configuration)
                    .AddOpenApiScalarDocumentation(configuration)
                    .AddLogServiceDependencies()
                    .AddNotificationControl()
                    .AddRequestResponseCompress()
                    .AddResponseRequestConfiguration()
                    .AddEfCorePersistence(configuration)
                    .AddDependencyInjections()
                    .AddApiCustomResults()
                    .AddGlobalExceptionHandlerMiddleware(builder)
                    .AddFilterToSystemLogs()
                    .AddMinimalApiVersionsing()
                    .AddAppHealthChecks()
                    .AddApiAuthentication(configuration)
                    .AddContractJsonOptions();

    #endregion

    builder.Services.AddOpenTelemetry()
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
        });

    var app = builder.Build();

    #region configuracoes dos middlewares

    app.UseResponseCompression()
     .UseExceptionHandler()
     .UseMiddleware<SerilogRequestLoggerMiddleware>()
     .UseHealthChecks(configuration)
     .UseHttpsRedirection()
     .UseMiddleware<TrimPropertiesContractMiddleware>();

    #region caso haja autenticação na api descomentar o código 

    app.UseAuthenticationAndAuthorizationMiddlewares(configuration);

    #endregion

    app.UseHealthChecksMiddleware(configuration);

    #endregion

    app.MapOpenApi();
    app.MapNomeContextoEndpoints(configuration);

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminado inexperadamente.");
}
finally
{
    await Log.CloseAndFlushAsync();
}
