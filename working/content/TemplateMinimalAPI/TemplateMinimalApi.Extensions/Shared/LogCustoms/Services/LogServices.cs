namespace TemplateMinimalApi.Extensions.Shared.LogCustoms.Services;

public class LogServices : ILogServices
{
    public LogData LogData { get; set; } = new();

    private readonly Serilog.ILogger _logger = Log.ForContext<LogServices>();

    public void WriteLog()
    {
        _logger.Information("[LogRequisição]:{mensagem} [RequestData]:{@RequestData} [RequestQuery]:{RequestQuery} [ResponseData]:{@ResponseData}" +
       "[Timestamp]:{Timestamp}", LogData.Mensagem, LogData.RequestData, LogData.RequestQuery, LogData.ResponseData, LogData.Timestamp);

        LogData.ClearLogData();
    }

    public void WriteLogWhenRaiseExceptions()
    {
        if (LogData is not null && LogData.Exception is not null)
        {
            _logger.Error("[ExceptionType]:{Name} [ExceptionMessage]:{Message}",
                LogData.Exception.GetType().Name, LogData.Exception.Message);

            _logger.Error($"[ExceptionStackTrace]:{LogData.Exception.StackTrace}");

            if (LogData?.Exception?.InnerException is not null)
            {
                _logger.Error("[InnerException]:{LogData.Exception?.InnerException?.Message}");
            }

            LogData.ClearLogExceptionData();
        }
    }

    public void CreateStructuredLog(LogData logData) => LogData = logData;

    public void WriteMessage(string message) => _logger.Information($"{message}");

    public void WriteErrorLog()
    {
        _logger.Error("[LogRequisição]:{mensagem} [RequestData]:{@RequestData}   [RequestQuery]:{RequestQuery}" +
            "[Method]:{RequestMethod} [Path]:{RequestUri} [RequestTraceId]:{TraceId} " +
            "[ResponseData]:{@ResponseData} [ResponseStatusCode]:{@ResponseStatusCode}",
            LogData.Mensagem, LogData.RequestData, LogData.RequestQuery, LogData.RequestMethod, LogData.RequestUri,
            LogData.TraceId, LogData.ResponseData, LogData.ResponseStatusCode);

        LogData.ClearLogData();
    }

    public void WriteLogFromResiliences()
    {
        _logger.Information("[LogRequisição]:{mensagem} [RequestUri]:{RequestUri} [ResponseStatusCode]:{ResponseStatusCode}  [RequestData]:{@RequestData}  [RequestQuery]:{RequestQuery} [ResponseData]:{@ResponseData}" +
         "[Timestamp]:{Timestamp}", LogData.Mensagem, LogData.RequestUri, LogData.ResponseStatusCode, LogData.RequestData, LogData.RequestQuery, LogData.ResponseData, LogData.Timestamp);

        LogData.ClearLogData();
    }

    public void WriteStaticMessage(string? message) => _logger.Information($"{message}");

    public void WriteContainerLog(string? message)
    {
        var dataAtual = DateTimeExtensions.GetGmtDateTime();

        Console.WriteLine($"[{dataAtual.ToString("HH:mm:ss")} INF] {message}");
    }
}
