var builder = WebApplication.CreateBuilder(args);

// 💡 This is where we add services to the container to create what amounts to a big list of services that
// can be used throughout our api. Services are objects that can be injected into other objects
// For example, we can add a WeatherForecastService to the container and then inject that service into our
// endpoint to get the weather forecast data

/* 💡here we are adding the WeatherForecastService as a service in our service container.
that means that we can inject IWeatherForecastService into our endpoints
and that .NET will automatically create an instance of the WeatherForecastService implementation
for us to use in our endpoint

The scoped lifetime means that a new instance of WeatherForecastService will be created
for each HTTP request that comes in. This is the most common lifetime for services.
There are other lifetimes like Singleton (one instance for the whole application)
and Transient (a new instance for each time it's requested) -- these will be needed 
pretty infrequently
*/
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* 💡this is a minimal API endpoint and one of the two ways you can define an endpoint that 
exposes logic in your application. There other option is to use controllers, which are probably
going to be your best bet for your use case, but this example will help illustrate how to expose 
an endpoint that you can hit in your angular app 
*/
app.MapGet("/weatherforecast", (IWeatherForecastService weatherForecastService) =>
    {
        return weatherForecastService.GetWeatherForecasts();
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();


// usually all this stuff below would be separated into different files, but I'm putting it all here for simplicity

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

/*
💡 This is the interface that we are going to use to register a service that can do the weather service work and
then inject that service into our endpoint (ie WeatherForecastService).
This interface is essentially an object we can use to say that we will be passing in kind of object that implements
this interface definition (ie something that can implement the GetWeatherForecasts method). In our case,
we will have a WeatherForecastService that implements this interface and can be injected into our endpoint, but you
could have other services that implement this interface and be injected into the endpoint as well
 */
public interface IWeatherForecastService
{
    WeatherForecast[] GetWeatherForecasts();
}

/*
💡 This is the service that we are going to use to get the weather forecast data. This service implements the
IWeatherForecastService interface, which means that it must have a GetWeatherForecasts method that returns an array
of WeatherForecast objects. This service is what will actually be injected into our endpoint and used to get the weather
because of how we registered the service collection
 */
public class WeatherForecastService() : IWeatherForecastService
{
    public WeatherForecast[] GetWeatherForecasts()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    }
}