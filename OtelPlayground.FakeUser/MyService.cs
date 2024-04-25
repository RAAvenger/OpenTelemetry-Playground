using Microsoft.Extensions.Logging;
using OtelPlayground.FakeUser.Utilities.Abstraction;

internal class MyService
{
    private static readonly string[] cities = ["zahedan", "birjand", "hamedan", "zanjan"];
    private readonly IActivitySourceWrapper _activitySource;
    private readonly ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger, IActivitySourceWrapper activitySource)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _activitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
    }

    public async Task GetAll()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            try
            {
                var response = await new HttpClient().GetAsync($"http://service-a/WeatherForecast/GetAllWeatherForecasts");
                response.EnsureSuccessStatusCode();
                var message = await response.Content.ReadAsStringAsync();
                _logger.GetAll(message);
            }
            catch (Exception ex)
            {
                _logger.GetAllError(ex);
            }
        }
    }

    public async Task GetRandomCity()
    {
        while (true)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            var city = cities[Random.Shared.Next(0, cities.Length)];
            try
            {
                var response = await new HttpClient().GetAsync($"http://service-a/WeatherForecast/GetWeatherForecast/{city}");
                response.EnsureSuccessStatusCode();
                var message = await response.Content.ReadAsStringAsync();
                _logger.GetCity(city, message);
            }
            catch (Exception ex)
            {
                _logger.GetCityError(city, ex);
            }
        }
    }
}