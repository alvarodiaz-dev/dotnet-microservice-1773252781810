using AutoMapper;
using WeatherForecast.Application.UseCases.GetWeatherForecast;
using WeatherForecast.Domain.Entities;

namespace WeatherForecast.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Weather, WeatherDayForecast>()
            .ConstructUsing(src => new WeatherDayForecast(
                src.ForecastDate,
                src.Temperature.Celsius,
                src.Temperature.Fahrenheit,
                src.Description,
                src.HumidityPercent,
                src.WindSpeedKmh
            ));
    }
}
