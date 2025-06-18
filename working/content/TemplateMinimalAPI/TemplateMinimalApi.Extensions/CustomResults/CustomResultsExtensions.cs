namespace TemplateMinimalApi.Extensions.CustomResults;

public static class CustomResultsExtensions
{
    public static IServiceCollection AddApiCustomResults(this IServiceCollection services)
    {
        services.AddSingleton<IApiCustomResults, ApiCustomResults>();
        return services;
    }

    public static IServiceCollection AddContractJsonOptions(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        return services;
    }
}
