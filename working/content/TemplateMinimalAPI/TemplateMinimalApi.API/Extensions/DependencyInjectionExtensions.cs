using TemplateMinimalApi.Extensions.Shared.Services;

namespace TemplateMinimalApi.API.Extensions;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
    {
        services.AddMediator();
        services.AddTransient(typeof(TemplateMinimalApi.Extensions.Mediator.IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        /*Resolução de dependência
         * Scoped => Tempo de vida da requisição.(aconselhado usar em APIS)
         * Transient => Toda vez que for encontrado na DI, é instanciado (melhor usar em Worker service);
         * Singleton => Uma única Instância na aplicação.
         */

        services.AddScoped<IEmailServices, EmailServices>();

        return services;
    }
}
