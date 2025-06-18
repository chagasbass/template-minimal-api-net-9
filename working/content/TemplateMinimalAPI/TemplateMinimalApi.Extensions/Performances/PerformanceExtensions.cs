namespace TemplateMinimalApi.Extensions.Performances;

public static class PerformanceApiExtensions
{
    public static IServiceCollection AddRequestResponseCompress(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;

            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
        });

        services.Configure<BrotliCompressionProviderOptions>(brotliOptions =>
        {
            brotliOptions.Level = CompressionLevel.Fastest;
        });

        services.Configure<GzipCompressionProviderOptions>(gzipOptions =>
        {
            gzipOptions.Level = CompressionLevel.Fastest;
        });

        return services;
    }

    public static IServiceCollection AddResponseRequestConfiguration(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(opcoes =>
        {
            var serializerOptions = opcoes.JsonSerializerOptions;
            serializerOptions.WriteIndented = true;
        });

        return services;
    }

}
