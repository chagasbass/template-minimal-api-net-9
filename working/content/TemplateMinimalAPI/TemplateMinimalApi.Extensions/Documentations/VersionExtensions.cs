namespace TemplateMinimalApi.Extensions.Documentations;

public static class VersionExtensions
{
    public static ApiVersionSet RetornarVersaoDeEndpoints(WebApplication app)
    {
        return app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1, 0))
        .HasApiVersion(new ApiVersion(2, 0))
        .ReportApiVersions()
        .Build();
    }
}