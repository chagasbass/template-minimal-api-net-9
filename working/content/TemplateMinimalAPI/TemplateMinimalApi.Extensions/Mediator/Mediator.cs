namespace TemplateMinimalApi.Extensions.Mediator;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        dynamic? handler = serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException($"Nenhum handler registrado para {requestType.Name}");
        }

        var behaviors = GetBehaviors<TResponse>(requestType);

        RequestHandlerDelegate<TResponse> handlerDelegate = () => handler.Handle((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle((dynamic)request, next, cancellationToken);
        }

        return handlerDelegate();
    }

    private IEnumerable<dynamic> GetBehaviors<TResponse>(Type requestType)
    {
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = serviceProvider.GetServices(behaviorType);

        return behaviors.Cast<dynamic>();
    }
}