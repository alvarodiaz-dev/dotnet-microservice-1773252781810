using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain.Entities;
using WeatherForecast.Domain.ValueObjects;

namespace WeatherForecast.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Weather> Weathers => Set<Weather>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Weather>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.OwnsOne(e => e.Location, location =>
            {
                location.Property(l => l.City).HasColumnName("City").HasMaxLength(100).IsRequired();
                location.Property(l => l.Country).HasColumnName("Country").HasMaxLength(100).IsRequired();
            });

            entity.OwnsOne(e => e.Temperature, temp =>
            {
                temp.Property(t => t.Celsius).HasColumnName("TemperatureCelsius").IsRequired();
            });

            entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
            entity.Property(e => e.HumidityPercent).IsRequired();
            entity.Property(e => e.WindSpeedKmh).IsRequired();
            entity.Property(e => e.ForecastDate).IsRequired();
            entity.Property(e => e.RecordedAt).IsRequired();

            entity.HasIndex(e => e.ForecastDate);
        });
    }
}
