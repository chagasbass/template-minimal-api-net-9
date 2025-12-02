namespace TemplateMinimalApi.Extensions.Documentations;

public static class OpenApiScalarExtensions
{
    public static IServiceCollection AddOpenApiScalarDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        return services;
    }
}
