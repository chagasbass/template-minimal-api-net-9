namespace TemplateMinimalApi.Extensions.Options;

public static class OptionsExtensions
{
    public static IServiceCollection AddBaseConfigurationOptionsPattern(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BaseConfigurationOptions>(configuration.GetSection(BaseConfigurationOptions.BaseConfig));
        services.Configure<DocumentationConfigurationOptions>(configuration.GetSection(DocumentationConfigurationOptions.DocumentationConfig));
        services.Configure<ProblemDetailConfigurationOptions>(configuration.GetSection(ProblemDetailConfigurationOptions.ProblemConfig));
        services.Configure<ResilienceConfigurationOptions>(configuration.GetSection(ResilienceConfigurationOptions.ResilienceConfig));
        services.Configure<EmailConfigurationOptions>(configuration.GetSection(EmailConfigurationOptions.EmailConfig));

        return services;
    }
}
