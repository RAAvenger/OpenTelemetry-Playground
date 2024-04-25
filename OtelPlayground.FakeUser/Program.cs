using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtelPlayground.FakeUser.Utilities;
using OtelPlayground.FakeUser.Utilities.Abstraction;

// See https://aka.ms/new-console-template for more information

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging((context, builder) =>
    {
        builder.AddOpenTelemetry(options =>
         {
             options.IncludeFormattedMessage = true;
             options.IncludeScopes = true;
             options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                    .AddService("fake user"));
             options.AddOtlpExporter();
         });
    });

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
        //.AddSource("fake user")
        .ConfigureResource(builder => builder.AddService("fake user"))
        .AddHttpClientInstrumentation()
        .AddOtlpExporter()
        .Build();
// Add services to the container.
builder.ConfigureServices(services =>
    {
        services.AddSingleton<MyService>();
        services.AddSingleton(tracerProvider.GetTracer("fake user"));
        services.AddSingleton<IActivitySourceWrapper, ActivitySourceWrapper>();
    });

var host = builder.Build();

var myService = host.Services.GetRequiredService<MyService>();

await Task.WhenAll(myService.GetAll(), myService.GetRandomCity());