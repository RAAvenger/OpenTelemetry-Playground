using Bogus;
using Microsoft.AspNetCore.Mvc;
using OtelPlayground.ServiceB.Commons;
using System.Diagnostics;

namespace OtelPlayground.ServiceB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetWeatherForecast/{algorithm}")]
        public ActionResult<WeatherForecast> Get([FromRoute] string algorithm, [FromQuery] string city)
        {
            using var activity = AppActivitySource.Instance.StartActivity("get weather for city with algorithm");
            activity.AddTag("city", city);
            activity.AddTag("algorithm", algorithm);
            return algorithm switch
            {
                "highp" => Ok(HighPerformanceMethod(city)),
                "lowp" => Ok(LowPerformanceMethod(city)),
                _ => NotFound($"given algorithm '{algorithm}' is not supported")
            };
        }

        private static WeatherForecast HighPerformanceMethod(string city)
        {
            return new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Summary = $"{city}'s weather is {Summaries[Random.Shared.Next(Summaries.Length)]}",
                TemperatureC = Random.Shared.Next(-20, 55)
            };
        }

        private static WeatherForecast LowPerformanceMethod(string city)
        {
            using var activity = AppActivitySource.Instance.StartActivity("get weather low performance algorithm");
            activity.AddEvent(new ActivityEvent("make faker"));
            var faker = new Faker<WeatherForecast>().RuleFor(x => x.Date, o => DateOnly.FromDateTime(DateTime.Now.AddDays(o.IndexFaker)))
                .RuleFor(x => x.TemperatureC, o => o.Random.Int(-20, 55))
                .RuleFor(x => x.Summary, o => o.Lorem.Paragraph(10));
            activity.AddEvent(new ActivityEvent("make 2000 data and take one"));
            var result = faker.Generate(2000).First();
            result.Summary = $"WeatherForecast for '{city}' is {result.Summary}";
            return result;
        }
    }
}