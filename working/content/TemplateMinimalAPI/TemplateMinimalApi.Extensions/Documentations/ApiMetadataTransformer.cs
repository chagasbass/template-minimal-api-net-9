using Microsoft.OpenApi;

namespace TemplateMinimalApi.Extensions.Documentations;

// Esta classe intercepta o documento OpenAPI antes dele ser renderizado
public class ApiMetadataTransformer(IOptions<DocumentationConfigurationOptions> optionsDelegate)
    : IOpenApiDocumentTransformer
{
    private readonly DocumentationConfigurationOptions _config = optionsDelegate.Value;

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info ??= new OpenApiInfo();

        var versionSuffix = context.DocumentName.ToUpperInvariant();

        document.Info.Title = !string.IsNullOrWhiteSpace(_config.NomeAplicacao)
            ? $"{_config.NomeAplicacao} {versionSuffix}"
            : $"API {versionSuffix}";

        document.Info.Version = context.DocumentName;

        var descricao = string.IsNullOrWhiteSpace(_config.Descricao) ? _config.Descricao : "Não informado";
        document.Info.Description = descricao;

        var desenvolvedor = string.IsNullOrWhiteSpace(_config.Desenvolvedor) ? _config.Desenvolvedor : "Não informado";

        document.Info.Contact = new OpenApiContact
        {
            Name = desenvolvedor
        };

        return Task.CompletedTask;
    }
}
