namespace TemplateMinimalApi.Extensions.Shared.Configurations;

public class BaseConfigurationOptions
{
    public const string BaseConfig = "BaseConfiguration";
    public string? NomeAplicacao { get; set; }
    public string? Descricao { get; set; }
    public string? NomeDesenvolvedor { get; set; }
    public string? StringConexaoBancoDeDados { get; set; }
    public bool TemAutenticacao { get; set; }
    public bool HabilitarMensagensDeLog { get; set; }

    public BaseConfigurationOptions() { }

}
