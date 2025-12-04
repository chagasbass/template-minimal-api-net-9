using Scalar.AspNetCore;

namespace TemplateMinimalApi.Extensions.Documentations;

public static class DocumentationExtensions
{
    public static IServiceCollection AddDocumentationVersioningConfig(this IServiceCollection services, List<string> versions)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        #region Add as novas versões de endpoints aqui
        versions.ForEach(version =>
        {
            services.AddOpenApi(version, options => { options.AddDocumentTransformer<ApiMetadataTransformer>(); });
        });

        #endregion

        return services;
    }
    public static IApplicationBuilder UseScalarDocumentation(this WebApplication app)
    {
        var options = app.Services.GetService<IOptions<DocumentationConfigurationOptions>>()?.Value;
        if (options is null) return app;

        var rotaDocs = string.IsNullOrWhiteSpace(options.RotaDocumentacao) ? "/scalar" : options.RotaDocumentacao;

        app.MapScalarApiReference(rotaDocs, scalarOptions =>
        {
            scalarOptions.WithTitle(options.NomeAplicacao ?? "Documentação da API");
            scalarOptions.WithTheme(ScalarTheme.Default);
            scalarOptions.WithSidebar(true);
            scalarOptions.WithDocumentDownloadType(DocumentDownloadType.Both);

            scalarOptions.WithOpenApiRoutePattern("/openapi/{documentName}.json");

            if (options.TemAutenticacao)
            {
                scalarOptions.AddPreferredSecuritySchemes("BearerAuth");
            }
            else
            {
                scalarOptions.HiddenClients = true;
            }
        });

        return app;
    }
}
