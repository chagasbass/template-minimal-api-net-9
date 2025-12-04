namespace TemplateMinimalApi.Extensions.Options;

public static class OptionsExtensions
{
    public static IServiceCollection AddBaseConfigurationOptionsPattern(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BaseConfigurationOptions>(configuration.GetSection(DocumentationConfigurationOptions.BaseConfig));
        services.Configure<DocumentationConfigurationOptions>(configuration.GetSection(DocumentationConfigurationOptions.BaseConfig));
        services.Configure<ProblemDetailConfigurationOptions>(configuration.GetSection(ProblemDetailConfigurationOptions.BaseConfig));
        services.Configure<HealthchecksConfigurationOptions>(configuration.GetSection(HealthchecksConfigurationOptions.BaseConfig));
        services.Configure<ResilienceConfigurationOptions>(configuration.GetSection(ResilienceConfigurationOptions.ResilienciaConfig));
        services.Configure<EmailConfigurationOptions>(configuration.GetSection(EmailConfigurationOptions.EmailConfig));

        return services;
    }
}
