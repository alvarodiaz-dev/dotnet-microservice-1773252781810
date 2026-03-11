using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Interfaces;
using WeatherForecast.Infrastructure.ExternalServices;
using WeatherForecast.Infrastructure.Persistence;
using WeatherForecast.Infrastructure.Repositories;

namespace WeatherForecast.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseInMemoryDatabase("WeatherForecastDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlServer(connectionString, sqlOpts =>
                {
                    sqlOpts.EnableRetryOnFailure(3);
                }));
        }

        services.AddScoped<IWeatherRepository, WeatherRepository>();
        services.AddScoped<IExternalWeatherProvider, ExternalWeatherProvider>();

        services.AddHttpClient();

        return services;
    }
}
