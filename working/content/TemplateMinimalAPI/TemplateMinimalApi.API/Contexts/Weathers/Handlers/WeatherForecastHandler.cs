namespace TemplateMinimalApi.API.Contexts.Weathers.Handlers;


/// <summary>
/// Exemplo de Handler para uso
/// </summary>
public class WeatherForecastHandler : IRequestHandler<WeatherForecastCommand, CommandResult>
{
    static List<string> summaries = new List<string>
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    public async Task<CommandResult> Handle(WeatherForecastCommand request, CancellationToken cancellationToken)
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
        {
            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(index));
            var temperature = Random.Shared.Next(-20, 55);
            var summary = summaries[Random.Shared.Next(summaries.Count)];
            return request.CreateTemperature(date, temperature, summary);
        })
     .ToImmutableList();

        return new CommandResult(forecast);
    }
}
