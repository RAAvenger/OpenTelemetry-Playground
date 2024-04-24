using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// See https://aka.ms/new-console-template for more information

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((context, builder) =>
    {
        builder.AddOpenTelemetry(options =>
         {
             options.IncludeFormattedMessage = true;
             options.IncludeScopes = true;
             options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService("fake user"))
             options.AddOtlpExporter();
         });
    });

// Add services to the container.
builder.ConfigureServices(services =>
    {
        services.AddOpenTelemetry()
            .WithTracing(options => options
                .SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService("fake user"))
                .AddHttpClientInstrumentation()
                .AddOtlpExporter());

        services.AddSingleton<MyService>();
    });

var host = builder.Build();

var myService = host.Services.GetRequiredService<MyService>();

await Task.WhenAll(myService.GetAll(), myService.GetRandomCity());