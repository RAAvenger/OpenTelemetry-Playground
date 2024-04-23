using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// See https://aka.ms/new-console-template for more information
var host = Host.CreateDefaultBuilder(args)
.ConfigureServices(services => { services.AddSingleton<MyService>(); })
.Build();

var myService = host.Services.GetRequiredService<MyService>();

await Task.WhenAll(myService.GetAll(), myService.GetRandomCity());