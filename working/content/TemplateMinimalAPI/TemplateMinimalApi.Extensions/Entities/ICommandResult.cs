namespace TemplateMinimalApi.Extensions.Entities;

public interface ICommandResult { }

public interface ICommandResult<T> : ICommandResult
{
    bool Success { get; set; }
    string? Message { get; set; }
    T? Data { get; set; }
}


