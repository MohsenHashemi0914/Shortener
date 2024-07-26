using ShortenerEndpoint;
using ShortenerEndpoint.Endpoints;
using ShortenerEndpoint.Extensions.DI;
using ShortenerEndpoint.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ShortenService>();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.AddMongoDb();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapShortenEndpoint();
app.MapRedirectEndpoint();

app.Run();