using OpenTelemetry.Metrics;
using Shortener.GrpcServices.Extensions;
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

builder.Services.AddShortenerGrpc();

builder.Services.AddOpenTelemetry()
                   .WithMetrics(builder =>
                   {
                       builder.AddPrometheusExporter();
                       builder.AddRuntimeInstrumentation();
                       builder.AddAspNetCoreInstrumentation();
                       
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
app.MapShortenerGrpcService<ShortenGrpcService>();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();