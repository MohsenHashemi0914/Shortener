using ShortenerEndpoint.Infrastructure.Contexts;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ShortenerEndpoint.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using ShortenerEndpoint.Observability.Metrics;

namespace ShortenerEndpoint.Services;

public sealed class ShortenService(
    ShortenDbContext dbContext,
    IOptions<AppSettings> settings,
    ShortenDiagnostic shortenDiagnostic)
{
    private readonly AppSettings _settings = settings.Value;
    private readonly ShortenDbContext _dbContext = dbContext;
    private readonly ShortenDiagnostic _shortenDiagnostic = shortenDiagnostic;

    public async Task<ErrorOr<string>> GetDestinationUrlAsync(string shortenCode, CancellationToken cancellationToken)
    {
        _shortenDiagnostic.AddRedirect();
        var urlLink = await _dbContext.UrlLinks.FirstOrDefaultAsync(x => x.ShortenCode == shortenCode, cancellationToken);
        if (urlLink is null)
        {
            return Error.NotFound(code: "NotFound", description: "Item not found.");
        }

        return urlLink.DestinationUrl;
    }

    public async Task<ErrorOr<string>> GetShortenUrlAsync(string longUrl, CancellationToken cancellationToken)
    {
        _shortenDiagnostic.AddShorten();
        var url = await _dbContext.UrlLinks.FirstOrDefaultAsync(x => x.DestinationUrl == longUrl, cancellationToken);

        if(url is not null)
        {
            return GetServiceUrl(url.ShortenCode);
        }

        var shortenCode = await GenerateShortenUrlAsync(longUrl, cancellationToken);
        if (shortenCode.IsError)
        {
            return shortenCode.FirstError;
        }

        var urlLink = new UrlLink
        {
            DestinationUrl = longUrl,
            CreatedOn = DateTime.UtcNow,
            ShortenCode = shortenCode.Value
        };

        await _dbContext.AddAsync(urlLink, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return GetServiceUrl(urlLink.ShortenCode);
    }

    private async Task<ErrorOr<string>> GenerateShortenUrlAsync(string longUrl, CancellationToken cancellationToken)
    {
        using var md5 = MD5.Create();

        var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(longUrl));
        var hashCode = BitConverter.ToString(hashBytes)
                       .Replace(oldValue: "-", newValue: string.Empty)
                       .ToLower();

        for (int i = 0; i <= hashCode.Length - 7; i++)
        {
            var shortenCode = hashCode.Substring(i, 7);
            var isRecordDuplicated = await _dbContext.UrlLinks.AnyAsync(x => x.ShortenCode == shortenCode, cancellationToken);
            if(isRecordDuplicated)
            {
                continue;
            }

            return shortenCode;
        }

        return Error.Conflict(code: "Duplicated item", description: "ShortenCode is duplicated");
    }

    private string GetServiceUrl(string shortenCode) => $"{_settings.BaseUrl}/{shortenCode}";
}