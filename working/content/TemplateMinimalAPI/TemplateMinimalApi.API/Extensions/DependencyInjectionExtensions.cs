using TemplateMinimalApi.Extensions.Shared.Services;

namespace TemplateMinimalApi.API.Extensions;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
    {
        //Configuração de handler
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Program>();

            //Colocar as pipeline em ordem de execução
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));

            //Exemplo configuração de Behaviors
            //  config.AddBehavior<IPipelineBehavior<ListarContasReceberParcelasCommandQuery, ICommandResult>,
            //ValidarContasAReceberBehaviors<ListarContasReceberParcelasCommandQuery, ICommandResult>>();

        });

        /*Resolução de dependência
         * Scoped => Tempo de vida da requisição.(aconselhado usar em APIS)
         * Transient => Toda vez que for encontrado na DI, é instanciado (melhor usar em Worker service);
         * Singleton => Uma única Instância na aplicação.
         */

        services.AddScoped<IEmailServices, EmailServices>();

        return services;
    }
}
