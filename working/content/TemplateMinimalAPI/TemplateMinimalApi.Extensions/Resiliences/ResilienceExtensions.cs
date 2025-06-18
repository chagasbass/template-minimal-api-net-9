namespace TemplateMinimalApi.Extensions.Resiliences;

public static class ResilienceExtensions
{
    public static IServiceCollection AddApiResiliencePatterns(this IServiceCollection services, IConfiguration configuration)
    {
        var quantidadeDeRetentativas = Int32.Parse(configuration["ResilienceConfiguration:QuantidadeDeRetentativas"]);
        var nomeCliente = configuration["ResilienceConfiguration:NomeCliente"];

        var serviceProvider = services.BuildServiceProvider();

        if (quantidadeDeRetentativas > 0)
        {
            services.AddHttpClient(nomeCliente, options =>
            {
                options.Timeout = TimeSpan.FromMinutes(5);
                options.DefaultRequestHeaders.Add("accept", "application/json");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler()
            {
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(10)
            })
             .SetHandlerLifetime(TimeSpan.FromMinutes(20))
             .AddPolicyHandler(ResiliencePolicies.GetApiRetryPolicy(serviceProvider, quantidadeDeRetentativas));

        }

        return services;
    }

    public static IServiceCollection AddTypedHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var nomeCliente = configuration["ResilienceConfiguration:NomeCliente"];

        nomeCliente ??= "restoqueWorker";

        services.AddHttpClient(nomeCliente, options =>
        {
            options.Timeout = TimeSpan.FromSeconds(80);
            options.DefaultRequestHeaders.Add("accept", "application/json");
        })
         .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler()
         {
             PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5)
         })
        .SetHandlerLifetime(TimeSpan.FromMinutes(20));

        return services;
    }
}
