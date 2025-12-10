namespace TemplateMinimalApi.Extensions.Mediator;

public static class MediatorServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();

        return services;
    }

    public static IServiceCollection AddRequestHandler<THandler, TRequest, TResponse>(
       this IServiceCollection services,
       ServiceLifetime lifetime = ServiceLifetime.Scoped)
       where THandler : class, IRequestHandler<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        services.Add(new ServiceDescriptor(typeof(IRequestHandler<TRequest, TResponse>), typeof(THandler), lifetime));

        return services;
    }
}
