namespace TemplateMinimalApi.API.Contextos.NomeContexto.Endpoints;

public static class NomeContextoEndpoints
{
    public static void MapNomeContextoEndpoints(this IEndpointRouteBuilder app, IConfiguration configuration)
    {
        var hasAuthentication = bool.TryParse(configuration["BaseConfiguration:TemAutenticacao"], out var auth) && auth;
        var versionamento = CarterVersionExtensions.VersionEndpoints(app);

        app.MapGet("/v{version:apiVersion}/endpoint-exemplo", async (IApiCustomResults customResults) =>
        {
            var commandResult = new CommandResult("Estou vivo", true);
            return customResults.FormatApiResponse(commandResult, "");
        })
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
        .WithName("Endpoint de teste")
        .WithTags("Teste - Meu Endpoint")
        .WithOpenApi()
        .RequireAuthorizationConditionally(hasAuthentication)
        .WithApiVersionSet(versionamento)
        .MapToApiVersion(1);
    }
}
