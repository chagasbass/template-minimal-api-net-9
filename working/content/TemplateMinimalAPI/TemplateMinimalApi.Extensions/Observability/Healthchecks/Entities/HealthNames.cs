namespace TemplateMinimalApi.Extensions.Observability.Healthchecks.Entities;

/// <summary>
/// Classe que contém as informações de Resource
/// </summary>
public static class HealthNames
{
    public static readonly string MemoryHealthcheck = "Informações de Processamento";
    public static readonly string SelfHealthcheck = "Self";
    public static readonly string MemoryDescription = "Consumo de memória permitido";
    public static readonly string MemoryDescriptionError = "Consumo de memória elevado";
    public static readonly string SelfDescription = "Monitoramento próprio";
    public static readonly string SelfDescriptionError = "Monitoramento próprio com erros";

    public static readonly List<string> MemoryTags = ["memória", "processos"];
    public static readonly List<string> SelfTags = ["self", "monitoramento"];
}
