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

    var versions = new List<string> { "v1", "v2" };

    builder.Services.AddEndpointsApiExplorer()
                    .AddProblemDetails()
                    .AddBaseConfigurationOptionsPattern(configuration)
                    .AddDocumentationVersioningConfig(versions)
                    .AddLogServiceDependencies()
                    .AddNotificationControl()
                    .AddRequestResponseCompress()
                    .AddResponseRequestConfiguration()
                    .AddDependencyInjections(configuration)
                    .AddApiCustomResults()
                    .AddGlobalExceptionHandlerMiddleware(builder)
                    .AddFilterToSystemLogs()
                    .AddAppHealthChecks()
                    .AddApiAuthentication(configuration)
                    .AddContractJsonOptions()
                    .AddOpenTelemetryExtension(configuration);

    #endregion

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
        app.MapOpenApi();
        app.UseScalarDocumentation();
    }

    app.MapWeathersEndpoints();

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
