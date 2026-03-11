using WeatherForecast.Domain.Entities;
using WeatherForecast.Domain.ValueObjects;

namespace WeatherForecast.Application.Interfaces;

public interface IExternalWeatherProvider
{
    Task<Weather?> GetCurrentWeatherAsync(Location location, CancellationToken cancellationToken = default);
    Task<IEnumerable<Weather>> GetForecastAsync(Location location, int days, CancellationToken cancellationToken = default);
}
