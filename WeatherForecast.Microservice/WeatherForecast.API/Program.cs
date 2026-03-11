using Serilog;
using WeatherForecast.Application;
using WeatherForecast.Infrastructure;
using WeatherForecast.API.Middleware;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, services, config) =>
        config
            .ReadFrom.Configuration(ctx.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/weatherforecast-.log", rollingInterval: RollingInterval.Day)
    );

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opts =>
    {
        opts.SwaggerDoc("v1", new()
        {
            Title = "WeatherForecast Microservice",
            Version = "v1",
            Description = "Clean Architecture weather forecast microservice"
        });
        opts.IncludeXmlComments(Path.Combine(
            AppContext.BaseDirectory,
            "WeatherForecast.API.xml"), failSilently: true);
    });

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherForecast v1"));
    }

    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
