using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Routing;

namespace TemplateMinimalApi.Extensions.EndpointModules;

public static class EndpointModulesExtensions { }

public static class ApiVersionExtensions
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
