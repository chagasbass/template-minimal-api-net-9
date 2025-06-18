namespace TemplateMinimalApi.API.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogServices logServices) :
                      IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logServices.WriteMessage("Operação iniciada");

        var result = await next();

        logServices.WriteMessage("Operação finalizada");

        return result;
    }
}
