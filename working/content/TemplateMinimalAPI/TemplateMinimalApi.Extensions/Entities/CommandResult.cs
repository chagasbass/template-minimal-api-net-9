namespace TemplateMinimalApi.Extensions.Entities;

public class CommandResult : ICommandResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("data")]
    public object? Data { get; set; }

    public CommandResult() { }

    public CommandResult(object? data, bool success = false, string? message = "")
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public CommandResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
