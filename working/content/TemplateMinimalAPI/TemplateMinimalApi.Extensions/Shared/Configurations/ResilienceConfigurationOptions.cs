namespace RestoqueTemplateMinimalApiApi.Shared.Configurations;

public class ResilienceConfigurationOptions
{
    public const string? ResilienciaConfig = "ResilienceConfiguration";
    public int QuantidadeDeRetentativas { get; set; }
    public string? NomeCliente { get; set; }

    public ResilienceConfigurationOptions() { }
}
