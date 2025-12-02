using Microsoft.EntityFrameworkCore;

namespace TemplateMinimalApi.API.Infra.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}

public static class EfCoreExtensions
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
