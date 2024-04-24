using Bogus;
using Microsoft.AspNetCore.Mvc;
using OtelPlayground.ServiceA.Controllers.Logs;

namespace OtelPlayground.ServiceA.Controllers
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

        [HttpGet("GetAllWeatherForecasts")]
        public ActionResult<IEnumerable<WeatherForecast>> GetAllWeatherForecasts()
        {
            if (Random.Shared.Next(1, 5) == 1)
            {
                try
                {
                    throw new Exception("random exception");
                }
                catch (Exception ex)
                {
                    _logger.CouldNotGenerateWhetherForecast(ex);
                    return Problem(ex.Message);
                }
            }
            var weather = LowPerformanceMethod();
            //var weather = HighPerformanceMethod();
            return base.Ok(weather);
        }

        [HttpGet("GetWeatherForecast/{city}")]
        public async Task<ActionResult<WeatherForecast>> GetWeatherForecast([FromRoute] string city)
        {
            int dice = Random.Shared.Next(1, 7);
            if (dice < 3)
            {
                return Ok(await LowPerformanceAlgorithm(city));
            }

            if (dice < 6)
            {
                return Ok(await HighPerformanceAlgorithm(city));
            }

            return Ok(await InvalidAlgorithm(city));
        }

        private static async Task<WeatherForecast?> HighPerformanceAlgorithm(string city)
        {
            var response = await new HttpClient().GetAsync($"http://service-b/WeatherForecast/GetWeatherForecast/highp?city={city}");
            return await response.Content.ReadFromJsonAsync<WeatherForecast>();
        }

        private static IReadOnlyCollection<WeatherForecast> HighPerformanceMethod()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private static async Task<WeatherForecast?> InvalidAlgorithm(string city)
        {
            var response = await new HttpClient().GetAsync($"http://service-b/WeatherForecast/GetWeatherForecast/highp?city={city}");
            return await response.Content.ReadFromJsonAsync<WeatherForecast>();
        }

        private static async Task<WeatherForecast?> LowPerformanceAlgorithm(string city)
        {
            var response = await new HttpClient().GetAsync($"http://service-b/WeatherForecast/GetWeatherForecast/lowp?city={city}");
            return await response.Content.ReadFromJsonAsync<WeatherForecast>();
        }

        private static IReadOnlyCollection<WeatherForecast> LowPerformanceMethod()
        {
            var faker = new Faker<WeatherForecast>().RuleFor(x => x.Date, o => DateOnly.FromDateTime(DateTime.Now.AddDays(o.IndexFaker)))
                .RuleFor(x => x.TemperatureC, o => o.Random.Int(-20, 55))
                .RuleFor(x => x.Summary, o => o.Lorem.Paragraph(10));
            return faker.Generate(20);
        }
    }
}