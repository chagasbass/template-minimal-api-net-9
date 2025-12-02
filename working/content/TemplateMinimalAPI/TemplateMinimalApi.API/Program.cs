using TemplateMinimalApi.Extensions.Authentications;
using Scalar.AspNetCore;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Microsoft.Extensions.FileProviders;

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

    if (app.Environment.IsDevelopment())
    {
        app.UseStaticFiles();
        app.MapOpenApi();
        var scalarPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "scalar");
        var scalarProvider = new PhysicalFileProvider(scalarPath);
        app.UseDefaultFiles(new DefaultFilesOptions
        {
            FileProvider = scalarProvider,
            RequestPath = "/scalar",
            DefaultFileNames = new List<string> { "index.html" }
        });
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = scalarProvider,
            RequestPath = "/scalar"
        });

        app.MapGet("/scalar", () => Results.File(Path.Combine(scalarPath, "index.html"), "text/html"));
    }
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
