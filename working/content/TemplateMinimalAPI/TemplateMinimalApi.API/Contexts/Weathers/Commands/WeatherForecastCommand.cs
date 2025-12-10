namespace TemplateMinimalApi.API.Contexts.Weathers.Commands;

public class WeatherForecastCommand : IRequest<ICommandResult>
{
    public int TemperatureF { get; set; }
    public DateOnly Date { get; set; }
    public string? Summary { get; set; }

    public WeatherForecastCommand CreateTemperature(DateOnly date, int temperatureC, string? summary)
    {
        Date = date;
        TemperatureF = 32 + (int)(temperatureC / 0.5556);
        Summary = summary;

        return new WeatherForecastCommand { Date = Date, TemperatureF = TemperatureF, Summary = Summary };
    }
}
