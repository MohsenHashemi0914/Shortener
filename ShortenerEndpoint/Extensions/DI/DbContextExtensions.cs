using Microsoft.EntityFrameworkCore;
using ShortenerEndpoint.Infrastructure.Contexts;

namespace ShortenerEndpoint.Extensions.DI;

public static class DbContextExtensions
{
    public static void AddMongoDb(this IHostApplicationBuilder builder)
    {
        var mongoDbConfig = builder.Configuration.GetSection(MongoDbOptions.SectionName).Get<MongoDbOptions>();

        ArgumentNullException.ThrowIfNull(mongoDbConfig, nameof(MongoDbOptions));

        builder.Services.AddDbContext<ShortenDbContext>(options =>
        {
            options.UseMongoDB(mongoDbConfig.Host, mongoDbConfig.DatabaseName);
        });
    }
}