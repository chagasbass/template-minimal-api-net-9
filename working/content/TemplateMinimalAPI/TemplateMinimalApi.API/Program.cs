using TemplateMinimalApi.Extensions.Authentications;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

Log.Logger = LogIntegrationsExtensions.ConfigureStructuralLogWithSerilog(configuration);
builder.Logging.AddSerilog(Log.Logger);

try
{
    #region configuracoes das extensoes

    builder.Services.AddEndpointsApiExplorer()
                    .AddProblemDetails()
                    .AddBaseConfigurationOptionsPattern(configuration)
                    .AddSwaggerDocumentation(configuration)
                    .AddLogServiceDependencies()
                    .AddNotificationControl()
                    .AddRequestResponseCompress()
                    .AddResponseRequestConfiguration()
                    .AddDependencyInjections()
                    .AddApiCustomResults()
                    .AddGlobalExceptionHandlerMiddleware(builder)
                    .AddFilterToSystemLogs()
                    .AddMinimalApiVersionsing()
                    .AddEndpointModuleExtensions()
                    .AddAppHealthChecks()
                    .AddApiAuthentication(configuration)
                    .AddContractJsonOptions();

    #endregion

    var app = builder.Build();

    #region configuracoes dos middlewares

    app.UseResponseCompression()
     .UseExceptionHandler()
     .UseMiddleware<SerilogRequestLoggerMiddleware>()
     .UseSwagger()
     .UseSwaggerUI()
     .UseHealthChecks(configuration)
     .UseHttpsRedirection()
     .UseMiddleware<TrimPropertiesContractMiddleware>();

    #region caso haja autenticação na api descomentar o código 

    app.UseAuthenticationAndAuthorizationMiddlewares(configuration);

    #endregion

    app.UseHealthChecksMiddleware(configuration);

    #endregion

    app.MapEndpointModules();

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