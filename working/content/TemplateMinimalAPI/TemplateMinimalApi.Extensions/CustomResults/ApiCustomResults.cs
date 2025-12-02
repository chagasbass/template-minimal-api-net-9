namespace TemplateMinimalApi.Extensions.CustomResults;

public class ApiCustomResults(ILogServices logServices, INotificationServices notificationServices) : IApiCustomResults
{
    public IResult FormatApiResponse(CommandResult commandResult, string? defaultEndpointRoute = null)
    {
        var statusCodeOperation = notificationServices.StatusCode;

        ICommandResult result = default;

        if (notificationServices.HasNotifications())
        {
            result = CreateErrorResponse(commandResult, statusCodeOperation.Id);

            notificationServices.ClearNotifications();
        }

        switch (statusCodeOperation)
        {
            case var _ when statusCodeOperation == StatusCodeOperation.BadRequest:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.BadRequest);
                return Results.BadRequest(result);
            case var _ when statusCodeOperation == StatusCodeOperation.NotFound:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.NotFound);
                return Results.NotFound(commandResult);
            case var _ when statusCodeOperation == StatusCodeOperation.Unauthorized:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.Unauthorized);
                return Results.Unauthorized();
            case var _ when statusCodeOperation == StatusCodeOperation.BusinessError:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.UnprocessableEntity);
                return Results.UnprocessableEntity(commandResult);
            case var _ when statusCodeOperation == StatusCodeOperation.Created:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.Created);
                return Results.Created(defaultEndpointRoute, commandResult);
            case var _ when statusCodeOperation == StatusCodeOperation.NoContent:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.NoContent);
                return Results.NoContent();
            case var _ when statusCodeOperation == StatusCodeOperation.OK:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.OK);
                return Results.Ok(commandResult);
            case var _ when statusCodeOperation == StatusCodeOperation.Accepted:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.Accepted);
                return Results.Ok(commandResult);
            default:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.OK);
                return Results.Ok(commandResult);
        }
    }

    public void GenerateLogResponse(CommandResult commandResult, int statusCode)
    {
        logServices.LogData.AddResponseStatusCode(statusCode)
                            .AddResponseBody(commandResult);

        logServices.WriteLog();
    }

    public ICommandResult CreateErrorResponse(CommandResult commandResult, int statusCode)
    {
        var notifications = notificationServices.GetNotifications();

        var defaultTitle = "Verifique o processamento do request.";

        var problemDetails = new MinimalApiProblemDetail(notifications.ToList(), commandResult.Message, statusCode, defaultTitle);

        commandResult.Data = problemDetails;

        return commandResult;
    }
}
