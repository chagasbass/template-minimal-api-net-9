namespace TemplateMinimalApi.API.Contextos.NomeContexto.Endpoints;


public static class WeathersEndpoints
{
    static List<string> summaries = new List<string>
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
    public static void MapWeathersEndpoints(this WebApplication app)
    {
        var apiVersionSet = VersionExtensions.RetornarVersaoDeEndpoints(app);

        app.MapGet("/v{version:apiVersion}/weatherforecast", async (IMediator mediator, IApiCustomResults customResults) =>
        {
            var commandResult = await mediator.Send(new WeatherForecastCommand());

            return await customResults.FormatApiResponse(commandResult);
        })
           .Produces<IEnumerable<WeatherForecastCommand>>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
           .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
           .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
           .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
           .WithName("WeathersV1")
           .WithTags("WeathersV1")
           .WithDescription(@"Endpoint responsável por exibir a previsão do tempo versão 1.")
           .WithSummary("Endpoint de Teste de aplicação V1")
           .WithApiVersionSet(apiVersionSet)
           .MapToApiVersion(new ApiVersion(1, 0));

        app.MapGet("/v{version:apiVersion}/weatherforecast", async (IMediator mediator, IApiCustomResults customResults) =>
        {
            //var forecast = Enumerable.Range(1, 5).Select(index =>
            //    new WeatherForecastCommand
            //    (
            //        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //        Random.Shared.Next(-20, 55),
            //        summaries[Random.Shared.Next(summaries.Count)]
            //    ))
            //    .ToImmutableList();
            //return forecast;
            var commandResult = await mediator.Send(new WeatherForecastCommand());

            return await customResults.FormatApiResponse(commandResult);
        })
         .Produces<IEnumerable<WeatherForecastCommand>>(StatusCodes.Status200OK)
         .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
         .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
         .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
         .WithName("WeathersV2")
         .WithTags("WeathersV2")
         .WithDescription(@"Endpoint responsável por exibir a previsão do tempo versão 2.(Precisa de autenticação)")
         .WithSummary("Endpoint de Teste de aplicação V2")
         .WithApiVersionSet(apiVersionSet)
         .MapToApiVersion(new ApiVersion(2, 0))
         .RequireAuthorization();
    }
}

//    public static void MapWeathersEndpoints(this IEndpointRouteBuilder app, IConfiguration configuration)
//    {
//        var hasAuthentication = bool.TryParse(configuration["BaseConfiguration:TemAutenticacao"], out var auth) && auth;
//        var versionamento = ApiVersionExtensions.VersionEndpoints(app);

//        app.MapGet("/v{version:apiVersion}/endpoint-exemplo", async (IApiCustomResults customResults) =>
//        {
//            var commandResult = new CommandResult("Estou vivo", true);
//            return customResults.FormatApiResponse(commandResult, "");
//        })
//        .Produces<string>(StatusCodes.Status200OK)
//        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
//        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
//        .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
//        .WithName("Endpoint de teste")
//        .WithTags("Teste - Meu Endpoint")
//        .AddOpenApiOperationTransformer((operation, context, ct) =>
//        {
//            operation.Summary = "Endpoint de teste";
//            operation.Description = "";
//            return Task.CompletedTask;
//        })
//        .RequireAuthorizationConditionally(hasAuthentication)
//        .WithApiVersionSet(versionamento)
//        .MapToApiVersion(1);


//}
