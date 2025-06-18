namespace TemplateMinimalApi.Extensions.Documentations.Versionings;

public static class VersioningExtensions
{
    public static IServiceCollection AddMinimalApiVersionsing(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
         {
             options.DefaultApiVersion = new ApiVersion(1, 0);
             options.ReportApiVersions = true;
             options.AssumeDefaultVersionWhenUnspecified = true;
             options.ReportApiVersions = true;

             #region add o versionamento no header
             //options.ApiVersionReader = ApiVersionReader.Combine(
             //                           new UrlSegmentApiVersionReader(),
             //                           new HeaderApiVersionReader("X-Api-Version"));
             #endregion

         }).AddApiExplorer(options =>
         {
             options.GroupNameFormat = "'v'V";
             options.SubstituteApiVersionInUrl = true;
         });

        return services;
    }
}
