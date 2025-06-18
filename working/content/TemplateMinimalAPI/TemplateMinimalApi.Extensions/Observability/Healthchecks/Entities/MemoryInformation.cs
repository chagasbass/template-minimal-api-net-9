namespace TemplateMinimalApi.Extensions.Observability.Healthchecks.Entities;

public class MemoryInformation : HealthData
{
    public string? MemoriaAlocada { get; set; }
    public string? TotalDeMemoriaDisponivel { get; set; }
    public string? MemoriaMaxima { get; set; }
    public string? SistemaOperacional { get; set; }
    public string? ArquiteturaDoSistemaOperacional { get; set; }
    public string? FrameworkDaAplicacao { get; set; }

    public MemoryInformation() { }

}
