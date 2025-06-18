namespace TemplateMinimalApi.Extensions.Observability.Healthchecks.Entities;

public class HealthInformation
{
    public string? Name { get; set; }
    public string? Data { get; set; }
    public string? Status { get; set; }
    public List<HealthData> HealthDatas { get; set; }
    public MemoryInformation? MemoryInformation { get; set; }

    public HealthInformation()
    {
        HealthDatas = [];
    }
}
