using Microsoft.Extensions.Logging;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Entities;
using WeatherForecast.Domain.ValueObjects;

namespace WeatherForecast.Infrastructure.ExternalServices;

/// <summary>
/// Stub implementation of an external weather provider.
/// Replace with a real HTTP client (e.g. OpenWeatherMap API) in production.
/// </summary>
public sealed class ExternalWeatherProvider : IExternalWeatherProvider
{
    private readonly ILogger<ExternalWeatherProvider> _logger;
    private static readonly Random _rng = new();

    public ExternalWeatherProvider(ILogger<ExternalWeatherProvider> logger) => _logger = logger;

    public async Task<Weather?> GetCurrentWeatherAsync(
        Location location,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching current weather for {Location} from external provider.", location);

        await Task.Delay(50, cancellationToken); // simulate network latency

        return BuildFakeWeather(location, DateTime.UtcNow.Date);
    }

    public async Task<IEnumerable<Weather>> GetForecastAsync(
        Location location,
        int days,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Fetching {Days}-day forecast for {Location} from external provider.", days, location);

        await Task.Delay(100, cancellationToken);

        return Enumerable.Range(0, days)
            .Select(i => BuildFakeWeather(location, DateTime.UtcNow.Date.AddDays(i)))
            .ToList();
    }

    private static Weather BuildFakeWeather(Location location, DateTime forecastDate) =>
        Weather.Create(
            location,
            Temperature.FromCelsius(Math.Round(_rng.NextDouble() * 35 - 5, 1)),
            PickDescription(),
            _rng.Next(30, 100),
            Math.Round(_rng.NextDouble() * 80, 1),
            forecastDate
        );

    private static string PickDescription()
    {
        var options = new[] { "Sunny", "Partly cloudy", "Overcast", "Light rain", "Thunderstorm", "Foggy", "Snowy" };
        return options[_rng.Next(options.Length)];
    }
}
