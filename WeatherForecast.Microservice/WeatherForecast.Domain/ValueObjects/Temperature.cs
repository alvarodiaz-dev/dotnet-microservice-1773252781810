namespace WeatherForecast.Domain.ValueObjects;

public sealed record Temperature
{
    public double Celsius { get; }
    public double Fahrenheit => Math.Round(Celsius * 9.0 / 5.0 + 32, 2);
    public double Kelvin => Math.Round(Celsius + 273.15, 2);

    private Temperature(double celsius) => Celsius = Math.Round(celsius, 2);

    public static Temperature FromCelsius(double celsius) => new(celsius);

    public static Temperature FromFahrenheit(double fahrenheit) =>
        new((fahrenheit - 32) * 5.0 / 9.0);

    public override string ToString() => $"{Celsius}°C / {Fahrenheit}°F";
}
