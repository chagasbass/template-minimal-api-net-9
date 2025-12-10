namespace TemplateMinimalApi.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEfCorePersistence(configuration);

        services.AddMediator();

        //resolvendo a dependência do pipeline de logging
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        //resolvendo a dependência do handler
        services.AddRequestHandler<WeatherForecastHandler, WeatherForecastCommand, ICommandResult>(ServiceLifetime.Scoped);

        /*Resolução de dependência
         * Scoped => Tempo de vida da requisição.(aconselhado usar em APIS)
         * Transient => Toda vez que for encontrado na DI, é instanciado (melhor usar em Worker service);
         * Singleton => Uma única Instância na aplicação.
         */

        services.AddScoped<IEmailServices, EmailServices>();

        return services;
    }
}
