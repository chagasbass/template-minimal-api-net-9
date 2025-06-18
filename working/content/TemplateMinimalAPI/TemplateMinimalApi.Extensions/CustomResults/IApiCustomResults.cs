namespace TemplateMinimalApi.Extensions.CustomResults;

public interface IApiCustomResults
{
    void GenerateLogResponse(CommandResult commandResult, int statusCode);
    IResult FormatApiResponse(CommandResult commandResult, string? defaultEndpoint = null);
}