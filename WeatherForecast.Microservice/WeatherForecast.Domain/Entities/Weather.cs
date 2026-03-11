using WeatherForecast.Domain.Common;
using WeatherForecast.Domain.ValueObjects;

namespace WeatherForecast.Domain.Entities;

public sealed class Weather : AggregateRoot
{
    public Location Location { get; private set; } = default!;
    public Temperature Temperature { get; private set; } = default!;
    public string Description { get; private set; } = string.Empty;
    public int HumidityPercent { get; private set; }
    public double WindSpeedKmh { get; private set; }
    public DateTime ForecastDate { get; private set; }
    public DateTime RecordedAt { get; private set; }

    private Weather() { }

    public static Weather Create(
        Location location,
        Temperature temperature,
        string description,
        int humidityPercent,
        double windSpeedKmh,
        DateTime forecastDate)
    {
        if (humidityPercent is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(humidityPercent), "Humidity must be between 0 and 100.");
        if (windSpeedKmh < 0)
            throw new ArgumentOutOfRangeException(nameof(windSpeedKmh), "Wind speed cannot be negative.");

        return new Weather
        {
            Location = location,
            Temperature = temperature,
            Description = description,
            HumidityPercent = humidityPercent,
            WindSpeedKmh = windSpeedKmh,
            ForecastDate = forecastDate,
            RecordedAt = DateTime.UtcNow
        };
    }
}
