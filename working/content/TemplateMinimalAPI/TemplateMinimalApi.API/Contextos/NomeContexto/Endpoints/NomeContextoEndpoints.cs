namespace TemplateMinimalApi.API.Contextos.NomeContexto.Endpoints;

public static class NomeContextoEndpoints
{
    public static void MapNomeContextoEndpoints(this IEndpointRouteBuilder app, IConfiguration configuration)
    {
        var hasAuthentication = bool.TryParse(configuration["BaseConfiguration:TemAutenticacao"], out var auth) && auth;
        var versionamento = ApiVersionExtensions.VersionEndpoints(app);

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
        .AddOpenApiOperationTransformer((operation, context, ct) =>
        {
            operation.Summary = "Endpoint de teste";
            operation.Description = "";
            return Task.CompletedTask;
        })
        .RequireAuthorizationConditionally(hasAuthentication)
        .WithApiVersionSet(versionamento)
        .MapToApiVersion(1);

        app.MapGet("/v{version:apiVersion}/efcore-demo", async (AppDbContext db, IApiCustomResults customResults) =>
        {
            var items = await db.DemoItems.OrderBy(x => x.Id).ToListAsync();
            var result = new CommandResult(items, true, "EF Core demo");
            return customResults.FormatApiResponse(result, "");
        })
        .Produces<IEnumerable<DemoItem>>(StatusCodes.Status200OK)
        .WithName("EF Core Demo")
        .WithTags("EFCore")
        .AddOpenApiOperationTransformer((operation, context, ct) =>
        {
            operation.Summary = "EF Core Demo";
            operation.Description = "";
            return Task.CompletedTask;
        })
        .RequireAuthorizationConditionally(hasAuthentication)
        .WithApiVersionSet(versionamento)
        .MapToApiVersion(1);

        app.MapPost("/v{version:apiVersion}/efcore-demo", async (AppDbContext db, IApiCustomResults customResults, INotificationServices notifications, DemoItem body) =>
        {
            db.DemoItems.Add(body);
            await db.SaveChangesAsync();
            var result = new CommandResult(body, true, "EF Core created");
            notifications.AddStatusCode(StatusCodeOperation.Created);
            var location = $"/v1/efcore-demo/{body.Id}";
            return customResults.FormatApiResponse(result, location);
        })
        .Produces<DemoItem>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .WithName("EF Core Create")
        .WithTags("EFCore")
        .AddOpenApiOperationTransformer((operation, context, ct) =>
        {
            operation.Summary = "EF Core Create";
            operation.Description = "";
            return Task.CompletedTask;
        })
        .RequireAuthorizationConditionally(hasAuthentication)
        .WithApiVersionSet(versionamento)
        .MapToApiVersion(1);
        app.MapPut("/v{version:apiVersion}/efcore-demo/{id:int}", async (int id, AppDbContext db, IApiCustomResults customResults, INotificationServices notifications, DemoItem body) =>
        {
            var entity = await db.DemoItems.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
            {
                notifications.AddStatusCode(StatusCodeOperation.NotFound);
                var notFound = new CommandResult(null, false, "Item não encontrado");
                return customResults.FormatApiResponse(notFound, "");
            }

            entity.Name = body.Name;
            await db.SaveChangesAsync();

            var result = new CommandResult(entity, true, "EF Core updated");
            return customResults.FormatApiResponse(result, "");
        })
        .Produces<DemoItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .WithName("EF Core Update")
        .WithTags("EFCore")
        .AddOpenApiOperationTransformer((operation, context, ct) =>
        {
            operation.Summary = "EF Core Update";
            operation.Description = "";
            return Task.CompletedTask;
        })
        .RequireAuthorizationConditionally(hasAuthentication)
        .WithApiVersionSet(versionamento)
        .MapToApiVersion(1);
        app.MapDelete("/v{version:apiVersion}/efcore-demo/{id:int}", async (int id, AppDbContext db, IApiCustomResults customResults, INotificationServices notifications) =>
        {
            var entity = await db.DemoItems.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
            {
                notifications.AddStatusCode(StatusCodeOperation.NotFound);
                var notFound = new CommandResult(null, false, "Item não encontrado");
                return customResults.FormatApiResponse(notFound, "");
            }

            db.DemoItems.Remove(entity);
            await db.SaveChangesAsync();

            notifications.AddStatusCode(StatusCodeOperation.NoContent);
            var result = new CommandResult(null, true, "EF Core deleted");
            return customResults.FormatApiResponse(result, "");
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .WithName("EF Core Delete")
        .WithTags("EFCore")
        .AddOpenApiOperationTransformer((operation, context, ct) =>
        {
            operation.Summary = "EF Core Delete";
            operation.Description = "";
            return Task.CompletedTask;
        })
        .RequireAuthorizationConditionally(hasAuthentication)
        .WithApiVersionSet(versionamento)
        .MapToApiVersion(1);
    }
}
