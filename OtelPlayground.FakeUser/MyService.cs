using Microsoft.Extensions.Logging;

public class MyService
{
    private static readonly string[] cities = ["zahedan", "birjand", "hamedan", "zanjan"];
    private readonly ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task GetAll()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            var response = await new HttpClient().GetAsync($"http://service-a/WeatherForecast/GetAllWeatherForecasts");
            var message = await response.Content.ReadAsStringAsync();
            _logger.GetAll(message);
        }
    }

    public async Task GetRandomCity()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            var city = cities[Random.Shared.Next(0, cities.Length)];
            var response = await new HttpClient().GetAsync($"http://service-a/WeatherForecast/GetWeatherForecast/{city}");
            var message = await response.Content.ReadAsStringAsync();
            _logger.GetCity(city, message);
        }
    }
}