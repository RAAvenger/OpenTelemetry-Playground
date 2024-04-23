{
    // See https://aka.ms/new-console-template for more information
    await Task.WhenAll( GetAll());
    return;
}

static async Task GetAll()
{
    while (true)
    {
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        var response = await new HttpClient().GetAsync($"http://service-a/WeatherForecast/GetAllWeatherForecasts");
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}

static async Task GetRandomCity()
{
    while (true)
    {
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        var response = await new HttpClient().GetAsync($"http://service-a/WeatherForecast/GetAllWeatherForecasts");
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}