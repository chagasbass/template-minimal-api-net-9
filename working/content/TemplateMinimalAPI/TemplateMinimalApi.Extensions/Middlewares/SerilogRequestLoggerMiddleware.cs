namespace TemplateMinimalApi.Extensions.Middlewares;

/// <summary>
/// Middleware que captura e estrutura dados de requisição e resposta
/// para emissão de logs via Serilog.
/// </summary>
public class SerilogRequestLoggerMiddleware(RequestDelegate next, ILogServices logServices)
{
    private static readonly HashSet<string> ExcludedContentTypes = new()
    {
        "application/openapi+json",
        "application/swagger+json"
    };

    private static readonly HashSet<string> ExcludedUserAgents = new(StringComparer.OrdinalIgnoreCase)
    {
        "scalar",
        "scalar-api-reference",
        "swagger-ui"
    };

    private static readonly HashSet<string> ExcludedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/scalar",
        "/swagger",
        "/openapi",
        "/swagger-ui",
        "/docs"
    };

    /// <summary>
    /// Lê o contexto HTTP, captura request e response, e emite logs estruturados.
    /// </summary>
    /// <param name="context">Contexto da requisição.</param>
    /// <returns>Tarefa assíncrona representando a execução do middleware.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var requestPath = context.Request.Path.ToString();
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var contentType = context.Request.ContentType ?? string.Empty;

        bool shouldSkipLogging =
            ExcludedPaths.Any(p => requestPath.StartsWith(p)) ||
            ExcludedUserAgents.Any(ua => userAgent.Contains(ua)) ||
            ExcludedContentTypes.Any(ct => contentType.Contains(ct));

        if (shouldSkipLogging)
        {
            await next(context);

            return;
        }


        string requestBody = await GetRequestBodyAsync(context);

        var originalBodyStream = context.Response.Body;

        ConsolidarInformacaoDeLogs(requestBody, context);

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await next(context);

            if (responseBody.CanSeek)
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);

                logServices.LogData.AddResponseStatusCode(context.Response.StatusCode)
                                    .AddResponseBody(responseText);
                logServices.WriteLog();
            }
        }
        catch (Exception ex)
        {
            logServices.LogData.AddException(ex)
                                .AddResponseStatusCode(context.Response.StatusCode);
            logServices.WriteLogWhenRaiseExceptions();
        }
        finally
        {
            if (responseBody.CanRead)
            {
                await responseBody.CopyToAsync(originalBodyStream);
            }
            context.Response.Body = originalBodyStream;
        }
    }

    /// <summary>
    /// Lê e bufferiza o corpo da requisição para permitir sua reutilização
    /// e registro em logs.
    /// </summary>
    /// <param name="httpContext">Contexto HTTP atual.</param>
    /// <returns>Corpo da requisição como texto.</returns>
    private static async Task<string> GetRequestBodyAsync(HttpContext httpContext)
    {
        var requestBody = string.Empty;

        if (httpContext.Request.ContentLength is null)
            return requestBody;

        HttpRequestRewindExtensions.EnableBuffering(httpContext.Request);

        Stream body = httpContext.Request.Body;

        byte[] buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];

        await httpContext.Request.Body.ReadExactlyAsync(buffer.AsMemory(0, buffer.Length));

        requestBody = Encoding.UTF8.GetString(buffer);

        body.Seek(0, SeekOrigin.Begin);

        httpContext.Request.Body = body;

        return requestBody;
    }

    /// <summary>
    /// Consolida informações relevantes da requisição no objeto de log.
    /// </summary>
    /// <param name="requestBody">Conteúdo do corpo da requisição.</param>
    /// <param name="httpContext">Contexto HTTP atual.</param>
    private void ConsolidarInformacaoDeLogs(string requestBody, HttpContext httpContext)
    {
        if (requestBody is null)
            return;

        if (httpContext.Request.QueryString.HasValue)
        {
            logServices.LogData.AddRequestQuery(httpContext.Request.QueryString.Value);
        }

        if (httpContext.Request.RouteValues.Count >= 2)
        {
            logServices.LogData.AddRequestQuery(httpContext.Request.Path);
        }

        logServices.LogData.AddRequestBody(requestBody)
                            .AddRequestType(httpContext.Request.Method)
                            .AddRequestUrl(httpContext.Request.Path)
                            .AddTraceIdentifier(httpContext.TraceIdentifier);
    }
}
