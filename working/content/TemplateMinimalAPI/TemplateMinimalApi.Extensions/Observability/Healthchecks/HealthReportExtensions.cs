namespace TemplateMinimalApi.Extensions.Observability.Healthchecks;

public static class HealthReportExtensions
{
    public static string AddHealthStatusData(this HealthReport report, IConfiguration configuration)
    {
        var applicationName = configuration["BaseConfiguration:NomeAplicacao"];

        var healthInformation = new HealthInformation
        {
            Name = applicationName,
            Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        };

        var entries = report.Entries.ToList();

        foreach (var entrie in entries)
        {
            if (entrie.Key.Equals(HealthNames.MemoryHealthcheck))
            {
                healthInformation.MemoryInformation = new MemoryInformation
                {
                    Nome = entrie.Key,
                    Descricao = entrie.Value.Description,
                    Status = entrie.Value.Status.ToString(),
                    MemoriaAlocada = GCInfoOptions.AllocatedMemory,
                    TotalDeMemoriaDisponivel = GCInfoOptions.TotalAvailableMemory,
                    MemoriaMaxima = GCInfoOptions.MaxMemory,
                    SistemaOperacional = GCInfoOptions.OperationalSystem,
                    ArquiteturaDoSistemaOperacional = GCInfoOptions.OperationalSystemArchitecture,
                    FrameworkDaAplicacao = GCInfoOptions.ApplicationFramework
                };
            }
            else
            {
                healthInformation.HealthDatas.Add(new HealthData
                {
                    Nome = entrie.Key,
                    Descricao = entrie.Value.Description,
                    Status = entrie.Value.Status.ToString()
                });
            }
        }

        var serializeOptions = JsonOptionsFactory.GetSerializerOptions();

        var healthResult = JsonSerializer.Serialize(healthInformation, serializeOptions);

        return healthResult;
    }
}
