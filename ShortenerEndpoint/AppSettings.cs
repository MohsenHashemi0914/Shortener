namespace ShortenerEndpoint;

public sealed class AppSettings
{
    public required string BaseUrl { get; set; }
    public required MongoDbOptions MongoDbConfigurations { get; set; }
}

public sealed class MongoDbOptions
{
    public const string SectionName = "MongoDbConfigurations";

    public required string Host { get; set; }
    public required string DatabaseName { get; set; }
}