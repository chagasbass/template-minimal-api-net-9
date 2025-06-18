using Asp.Versioning.Builder;
using Carter;
using Microsoft.AspNetCore.Routing;

namespace TemplateMinimalApi.Extensions.EndpointModules;

public static class EndpointModulesExtensions
{
    public static IServiceCollection AddEndpointModuleExtensions(this IServiceCollection services)
    {
        services.AddCarter();

        return services;
    }

    public static WebApplication MapEndpointModules(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}

public static class CarterVersionExtensions
{
    public static ApiVersionSet VersionEndpoints(IEndpointRouteBuilder app)
    {
        return app.NewApiVersionSet()
                   .HasApiVersion(new ApiVersion(1))
                   .ReportApiVersions()
                   .Build();
    }
}

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder RequireAuthorizationConditionally(
        this RouteHandlerBuilder builder,
        bool condition,
        params string[] policyNames)
    {
        return condition ? builder.RequireAuthorization(policyNames) : builder;
    }
}
