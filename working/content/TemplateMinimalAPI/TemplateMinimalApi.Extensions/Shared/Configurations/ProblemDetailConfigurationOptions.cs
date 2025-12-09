namespace TemplateMinimalApi.Extensions.Shared.Configurations;

public class ProblemDetailConfigurationOptions
{
    public const string ProblemConfig = "ProblemDetailConfiguration";
    public string? Title { get; set; }
    public string? Detail { get; set; }

    public ProblemDetailConfigurationOptions() { }
}
