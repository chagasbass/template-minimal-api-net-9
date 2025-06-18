namespace TemplateMinimalApi.Extensions.CustomLogs;

public static class StandardLogExtensions
{
    public static IServiceCollection AddMinimalApiAspNetCoreHttpLogging(this IServiceCollection services)
    {
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                                    HttpLoggingFields.ResponseStatusCode |
                                    HttpLoggingFields.ResponseBody |
                                    HttpLoggingFields.RequestBody;

            options.RequestBodyLogLimit = 4096;
            options.ResponseBodyLogLimit = 4096;
            options.MediaTypeOptions.AddText("application/json");
        });

        return services;
    }
}
