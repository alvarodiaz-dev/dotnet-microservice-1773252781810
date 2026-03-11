namespace WeatherForecast.Application.UseCases.GetWeatherForecast;

public sealed record GetWeatherForecastResponse(
    string City,
    string Country,
    IReadOnlyList<WeatherDayForecast> Forecasts
);

public sealed record WeatherDayForecast(
    DateTime ForecastDate,
    double TemperatureCelsius,
    double TemperatureFahrenheit,
    string Description,
    int HumidityPercent,
    double WindSpeedKmh
);
