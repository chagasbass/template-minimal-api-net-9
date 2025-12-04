namespace TemplateMinimalApi.Extensions.Shared.Configurations;

public class DocumentationConfigurationOptions
{
    public const string BaseConfig = "DocumentationConfiguration";
    public string? NomeAplicacao { get; set; }
    public string? Desenvolvedor { get; set; }
    public string? Descricao { get; set; }
    public string? RotaDocumentacao { get; set; }
    public bool TemAutenticacao { get; set; }

    public DocumentationConfigurationOptions() { }

}