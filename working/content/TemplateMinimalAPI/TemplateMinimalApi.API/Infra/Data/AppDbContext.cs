namespace TemplateMinimalApi.API.Infra.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DemoItem> DemoItems => Set<DemoItem>();
}