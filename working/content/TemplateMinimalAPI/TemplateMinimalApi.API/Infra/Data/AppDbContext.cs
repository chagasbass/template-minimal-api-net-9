using Microsoft.EntityFrameworkCore;

namespace TemplateMinimalApi.API.Infra.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DemoItem> DemoItems => Set<DemoItem>();
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
        else
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AppDb"));
        }
        return services;
    }
}
