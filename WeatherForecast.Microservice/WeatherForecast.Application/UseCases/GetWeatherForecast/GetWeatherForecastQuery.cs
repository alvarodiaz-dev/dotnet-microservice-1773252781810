using MediatR;

namespace WeatherForecast.Application.UseCases.GetWeatherForecast;

public sealed record GetWeatherForecastQuery(string City, string Country, int Days = 5)
    : IRequest<GetWeatherForecastResponse>;
