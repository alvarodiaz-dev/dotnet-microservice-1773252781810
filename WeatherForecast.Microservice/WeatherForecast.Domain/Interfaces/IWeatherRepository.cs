using WeatherForecast.Domain.Entities;
using WeatherForecast.Domain.ValueObjects;

namespace WeatherForecast.Domain.Interfaces;

public interface IWeatherRepository
{
    Task<Weather?> GetByLocationAsync(Location location, CancellationToken cancellationToken = default);
    Task<IEnumerable<Weather>> GetForecastAsync(Location location, int days, CancellationToken cancellationToken = default);
    Task AddAsync(Weather weather, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
