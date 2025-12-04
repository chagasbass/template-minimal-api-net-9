namespace TemplateMinimalApi.Extensions.Middlewares;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddGlobalExceptionHandlerMiddleware(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                if (builder.Environment.IsProduction())
                {
                    context.ProblemDetails.Extensions.Remove("exception");
                    context.ProblemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                }
            };
        });

        services.AddTransient<UnauthorizedTokenMiddleware>();

        return services;
    }
}
