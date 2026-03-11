using FluentValidation;
using WeatherForecast.Application.UseCases.GetWeatherForecast;

namespace WeatherForecast.Application.Validators;

public sealed class GetWeatherForecastQueryValidator : AbstractValidator<GetWeatherForecastQuery>
{
    public GetWeatherForecastQueryValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100).WithMessage("City must not exceed 100 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters.");

        RuleFor(x => x.Days)
            .InclusiveBetween(1, 14).WithMessage("Days must be between 1 and 14.");
    }
}
