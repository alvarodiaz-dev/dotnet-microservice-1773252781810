using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain.Entities;
using WeatherForecast.Domain.Interfaces;
using WeatherForecast.Domain.ValueObjects;
using WeatherForecast.Infrastructure.Persistence;

namespace WeatherForecast.Infrastructure.Repositories;

public sealed class WeatherRepository : IWeatherRepository
{
    private readonly ApplicationDbContext _context;

    public WeatherRepository(ApplicationDbContext context) => _context = context;

    public async Task<Weather?> GetByLocationAsync(
        Location location,
        CancellationToken cancellationToken = default)
    {
        return await _context.Weathers
            .Where(w =>
                EF.Property<string>(EF.Property<object>(w, "Location"), "City") == location.City &&
                EF.Property<string>(EF.Property<object>(w, "Location"), "Country") == location.Country)
            .OrderByDescending(w => w.RecordedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Weather>> GetForecastAsync(
        Location location,
        int days,
        CancellationToken cancellationToken = default)
    {
        var from = DateTime.UtcNow.Date;
        var to = from.AddDays(days);

        return await _context.Weathers
            .Where(w =>
                EF.Property<string>(EF.Property<object>(w, "Location"), "City") == location.City &&
                EF.Property<string>(EF.Property<object>(w, "Location"), "Country") == location.Country &&
                w.ForecastDate >= from && w.ForecastDate <= to)
            .OrderBy(w => w.ForecastDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Weather weather, CancellationToken cancellationToken = default) =>
        await _context.Weathers.AddAsync(weather, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
