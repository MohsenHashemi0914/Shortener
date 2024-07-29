using System.Diagnostics.Metrics;

namespace ShortenerEndpoint.Observability.Metrics;

public sealed class ShortenDiagnostic
{
    public const string MeterName = "ShortenerService";
    private const string ShortenName = "ShortenerService.Shorten";
    private const string RedirectName = "ShortenerService.Redirect";
    
    private readonly Counter<long> _shortenCounter;
    private readonly Counter<long> _redirectCounter;

    public ShortenDiagnostic(IMeterFactory factory)
    {
        var meter = factory.Create(MeterName);
        _shortenCounter = meter.CreateCounter<long>(ShortenName);
        _redirectCounter = meter.CreateCounter<long>(RedirectName);
    }

    public void AddShorten() => _shortenCounter.Add(1);

    public void AddRedirect() => _redirectCounter.Add(1);
}