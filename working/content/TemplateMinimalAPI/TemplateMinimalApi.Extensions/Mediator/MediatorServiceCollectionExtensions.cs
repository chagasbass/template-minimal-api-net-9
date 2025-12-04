namespace TemplateMinimalApi.Extensions.Mediator;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, Mediator>();

        return services;
    }
}
