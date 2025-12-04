namespace TemplateMinimalApi.API.Infra.Extensions;

public static class DataBaseExtensions
{
    public static IServiceCollection AddEfCorePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["BaseConfiguration:StringConexaoBancoDeDados"];

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }

        return services;
    }
}