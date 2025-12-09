namespace TemplateMinimalApi.Extensions.Resiliences;

public static class ResiliencePolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetApiRetryPolicy(IServiceProvider serviceProvider, int quantidadeDeRetentativas)
    {
        var quantidadeTotalDeRetentativas = quantidadeDeRetentativas;

        const string _retryMessage = "Retentativas de chamadas externas foram excedidas.";

        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode != HttpStatusCode.OK)
            .RetryAsync(quantidadeDeRetentativas, onRetry: (message, numeroDeRetentativas) =>
            {
                if (quantidadeTotalDeRetentativas == numeroDeRetentativas && message.Result is not null)
                {
                    var logServices = serviceProvider.GetService<ILogServices>();

                    logServices.LogData.AddMessageInformation(_retryMessage)
                                   .AddResponseStatusCode((int)message.Result.StatusCode)
                                   .AddRequestUrl(message.Result.RequestMessage.RequestUri.AbsoluteUri)
                                   .AddRequestBody(message.Result.RequestMessage.Content)
                                   .AddResponseBody(message.Result.Content);

                    logServices.WriteLogFromResiliences();
                }
            });
    }

    public static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetAsyncCircuitBreakerPolicy(IServiceProvider serviceProvider, int falhasPermitidas, int duracaoDoBreak)
    {
        var logServices = serviceProvider.GetService<ILogServices>();

        return Policy<HttpResponseMessage>
       .Handle<HttpRequestException>()
       .OrResult(r => !r.IsSuccessStatusCode)
       .CircuitBreakerAsync(
           handledEventsAllowedBeforeBreaking: falhasPermitidas,
           durationOfBreak: TimeSpan.FromSeconds(duracaoDoBreak),
           onBreak: (outcome, timespan, context) =>
           {
               string motivo = outcome.Exception?.Message ??
                              outcome.Result?.StatusCode.ToString() ??
                              "Desconhecido";
               logServices.WriteContainerLog($"Circuito ABERTO por {timespan.TotalSeconds} segundos");
               logServices.WriteContainerLog($"Motivo: {motivo}");
           },
           onReset: (context) =>
           {
               logServices.WriteContainerLog("Circuito FECHADO - Operações normais");
           },
           onHalfOpen: () =>
           {
               logServices.WriteContainerLog("Circuito MEIO-ABERTO - Testando próxima requisição");
           });
    }

    public static AsyncFallbackPolicy<HttpResponseMessage> GetApiFallbackPolicy(IServiceProvider serviceProvider, HttpResponseMessage respostaDeFallback)
    {
        var logServices = serviceProvider.GetService<ILogServices>();

        return Policy<HttpResponseMessage>
           .Handle<HttpRequestException>()
           .Or<TimeoutException>()
           .OrResult(msg => !msg.IsSuccessStatusCode)
           .FallbackAsync(
               fallbackAction: async (outcome, context, cancellationToken) =>
               {
                   logServices?.WriteContainerLog("Fallback acionado - retornando resposta alternativa");
                   return await Task.FromResult(respostaDeFallback);
               },
               onFallbackAsync: async (outcome, context) =>
               {
                   string motivo = outcome.Exception?.Message
                       ?? $"Status HTTP: {(int?)outcome.Result?.StatusCode}";
                   logServices?.WriteContainerLog($"Fallback necessário: {motivo}");
                   await Task.CompletedTask;
               });
    }

    public static AsyncTimeoutPolicy<HttpResponseMessage> GetApiTimeoutPolicy(IServiceProvider serviceProvider, int tempoDeTimeout)
    {
        var logServices = serviceProvider.GetService<ILogServices>();
        return Policy
            .TimeoutAsync<HttpResponseMessage>(
                timeout: TimeSpan.FromSeconds(tempoDeTimeout),
                onTimeoutAsync: (context, timespan, task) =>
                {
                    logServices.WriteContainerLog($"Timeout de {timespan.TotalSeconds} segundos excedido.");
                    logServices.WriteContainerLog($"Operação: {context.OperationKey}");

                    return Task.CompletedTask;
                });
    }
}