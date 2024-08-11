using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace ShortenerEndpoint.Observability.Metrics;

public sealed class ShortenDiagnostic
{
    public const string MeterName = "ShortenerService";
    private const string ShortenName = "ShortenerService.Shorten";
    private const string RedirectName = "ShortenerService.Redirect";
    private const string FailedRedirectName = "ShortenerService.FailedRedirect";
    
    private readonly Counter<long> _shortenCounter;
    private readonly Counter<long> _redirectCounter;
    private readonly Counter<long> _failedRedirectCounter;

    public ShortenDiagnostic(IMeterFactory factory)
    {
        var meter = factory.Create(MeterName);
        _shortenCounter = meter.CreateCounter<long>(ShortenName);
        _redirectCounter = meter.CreateCounter<long>(RedirectName);
        _failedRedirectCounter = meter.CreateCounter<long>(FailedRedirectName);
    }


    public void AddShorten() => _shortenCounter.Add(1);

    public void AddRedirect(string shortenCode) => _redirectCounter.Add(1, new TagList
    {
        new KeyValuePair<string, object?>("code", shortenCode)
    });

    public void AddFailedRedirect(string shortenCode) => _failedRedirectCounter.Add(1, new TagList
    {
        new KeyValuePair<string, object?>("code", shortenCode)
    });
}