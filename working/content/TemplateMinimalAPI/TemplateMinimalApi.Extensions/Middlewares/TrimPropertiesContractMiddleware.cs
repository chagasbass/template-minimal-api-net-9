namespace TemplateMinimalApi.Extensions.Middlewares;

public sealed class TrimPropertiesContractMiddleware(RequestDelegate next)
{
    JsonElement TrimProperties(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                using (var doc = JsonDocument.Parse(element.GetRawText()))
                {
                    var obj = new Dictionary<string, object>();
                    foreach (var property in doc.RootElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind == JsonValueKind.String)
                        {
                            // Remove espaços de strings
                            obj[property.Name] = property.Value.GetString()?.Trim();
                        }
                        else
                        {
                            // Processa propriedades recursivamente
                            obj[property.Name] = TrimProperties(property.Value);
                        }
                    }
                    return JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(obj));
                }

            case JsonValueKind.Array:
                var array = new List<object>();
                foreach (var item in element.EnumerateArray())
                {
                    array.Add(TrimProperties(item));
                }
                return JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(array));

            default:
                return element;
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            await next(context); // Chama o próximo middleware (ou a execução da rota)

            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

            // Verifica se o conteúdo é JSON
            if (context.Response.ContentType != null &&
                context.Response.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
            {
                // Converte a string JSON em objeto
                var jsonObject = JsonSerializer.Deserialize<JsonElement>(responseBody);

                // Remove espaços em branco das propriedades
                jsonObject = TrimProperties(jsonObject);

                // Serializa novamente o objeto modificado
                var modifiedJson = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                // Reescreve a resposta com o JSON modificado
                context.Response.Body = originalBodyStream;
                await context.Response.WriteAsync(modifiedJson);
            }
            else
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}
