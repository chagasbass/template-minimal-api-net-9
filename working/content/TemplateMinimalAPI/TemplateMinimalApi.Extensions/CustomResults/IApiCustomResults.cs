namespace TemplateMinimalApi.Extensions.CustomResults;

public interface IApiCustomResults
{
    Task<IResult> FormatApiResponse(CommandResult commandResult, string? defaultEndpointRoute = null);
    void GenerateLogResponse(CommandResult commandResult, int statusCode);
    ICommandResult CreateErrorResponse(CommandResult commandResult, int statusCode);
}