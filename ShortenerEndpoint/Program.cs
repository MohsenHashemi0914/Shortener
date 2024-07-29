using OpenTelemetry.Metrics;
using ShortenerEndpoint;
using ShortenerEndpoint.Endpoints;
using ShortenerEndpoint.Extensions.DI;
using ShortenerEndpoint.Observability.Metrics;
using ShortenerEndpoint.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ShortenService>();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.AddMongoDb();
builder.Services.AddSingleton<ShortenDiagnostic>();

builder.Services.AddOpenTelemetry()
                   .WithMetrics(builder =>
                   {
                       builder.AddPrometheusExporter();

                       var meterNames = new[]
                       {
                           ShortenDiagnostic.MeterName
                       };

                       builder.AddMeter(meterNames);
                   });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapShortenEndpoint();
app.MapRedirectEndpoint();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();