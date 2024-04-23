using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

// See https://aka.ms/new-console-template for more information

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((context, builder) =>
    {
        builder.AddOpenTelemetry(options =>
         {
             options.SetResourceBuilder(ResourceBuilder.CreateDefault())
                 .AddConsoleExporter();
         });
    })
    .ConfigureServices(services => { services.AddSingleton<MyService>(); });

var host = builder.Build();

var myService = host.Services.GetRequiredService<MyService>();

await Task.WhenAll(myService.GetAll(), myService.GetRandomCity());