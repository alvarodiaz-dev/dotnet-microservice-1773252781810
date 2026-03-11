using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Application.UseCases.GetWeatherForecast;

namespace WeatherForecast.API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class WeatherController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherController(IMediator mediator) => _mediator = mediator;

    /// <summary>Returns a weather forecast for the given city and country.</summary>
    /// <param name="city">City name (e.g. London)</param>
    /// <param name="country">Country name or code (e.g. UK)</param>
    /// <param name="days">Number of forecast days (1–14, default 5)</param>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    [ProducesResponseType(typeof(GetWeatherForecastResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetForecast(
        [FromQuery] string city,
        [FromQuery] string country,
        [FromQuery] int days = 5,
        CancellationToken cancellationToken = default)
    {
        var query = new GetWeatherForecastQuery(city, country, days);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
