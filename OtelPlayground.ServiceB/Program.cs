using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtelPlayground.ServiceB.Commons;

var builder = WebApplication.CreateBuilder(args); 
Console.WriteLine(builder.Configuration["ServiceName"]);

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
    options.SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(builder.Configuration["ServiceName"]!));

    options.AddOtlpExporter();
});

// Add services to the container.
builder.Services
    .AddOpenTelemetry()
    .WithMetrics(options => options.AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation()
        .AddPrometheusExporter())
    .WithTracing(options => options.SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddService(builder.Configuration["ServiceName"]!))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource(AppActivitySource.AppActivityName)
        .AddOtlpExporter());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();