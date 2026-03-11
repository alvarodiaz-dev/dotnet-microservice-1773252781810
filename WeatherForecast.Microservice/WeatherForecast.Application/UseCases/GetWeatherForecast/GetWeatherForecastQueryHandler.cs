using AutoMapper;
using MediatR;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Interfaces;
using WeatherForecast.Domain.ValueObjects;

namespace WeatherForecast.Application.UseCases.GetWeatherForecast;

public sealed class GetWeatherForecastQueryHandler
    : IRequestHandler<GetWeatherForecastQuery, GetWeatherForecastResponse>
{
    private readonly IWeatherRepository _repository;
    private readonly IExternalWeatherProvider _externalProvider;
    private readonly IMapper _mapper;

    public GetWeatherForecastQueryHandler(
        IWeatherRepository repository,
        IExternalWeatherProvider externalProvider,
        IMapper mapper)
    {
        _repository = repository;
        _externalProvider = externalProvider;
        _mapper = mapper;
    }

    public async Task<GetWeatherForecastResponse> Handle(
        GetWeatherForecastQuery request,
        CancellationToken cancellationToken)
    {
        var location = Location.Create(request.City, request.Country);

        var forecasts = (await _repository.GetForecastAsync(location, request.Days, cancellationToken)).ToList();

        if (forecasts.Count == 0)
        {
            forecasts = (await _externalProvider.GetForecastAsync(location, request.Days, cancellationToken)).ToList();

            foreach (var forecast in forecasts)
                await _repository.AddAsync(forecast, cancellationToken);

            await _repository.SaveChangesAsync(cancellationToken);
        }

        return new GetWeatherForecastResponse(
            location.City,
            location.Country,
            _mapper.Map<IReadOnlyList<WeatherDayForecast>>(forecasts)
        );
    }
}
