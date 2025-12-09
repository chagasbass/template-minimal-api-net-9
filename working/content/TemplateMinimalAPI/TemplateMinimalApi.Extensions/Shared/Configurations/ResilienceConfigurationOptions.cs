namespace RestoqueTemplateMinimalApiApi.Shared.Configurations;

public class ResilienceConfigurationOptions
{
    public const string? ResilienceConfig = "ResilienceConfiguration";
    public int QuantidadeDeRetentativas { get; set; }
    public int FalhasPermitidas { get; set; }
    public int DuracaoDoBreak { get; set; }
    public int TempoDeTimeout { get; set; }

    public ResilienceConfigurationOptions() { }
}
