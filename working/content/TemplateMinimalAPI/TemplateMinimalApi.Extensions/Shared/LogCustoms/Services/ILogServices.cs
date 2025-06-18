namespace TemplateMinimalApi.Extensions.Shared.LogCustoms.Services;

public interface ILogServices
{
    public LogData LogData { get; set; }
    void WriteLog();
    void WriteErrorLog();
    void CreateStructuredLog(LogData logData);
    void WriteLogWhenRaiseExceptions();
    void WriteMessage(string message);
    void WriteStaticMessage(string? message);
    void WriteLogFromResiliences();
    void WriteContainerLog(string message);
}
