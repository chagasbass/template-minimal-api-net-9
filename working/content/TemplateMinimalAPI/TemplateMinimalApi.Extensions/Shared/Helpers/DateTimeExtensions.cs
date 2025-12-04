namespace TemplateMinimalApi.Extensions.Shared.Helpers;

public static class DateTimeExtensions
{
    public static DateTime GetGmtDateTime()
    {
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
    }
}
