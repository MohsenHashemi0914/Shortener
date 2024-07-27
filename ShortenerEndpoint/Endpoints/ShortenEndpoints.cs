using Microsoft.AspNetCore.Mvc;
using ShortenerEndpoint.Services;

namespace ShortenerEndpoint.Endpoints;

public static class ShortenEndpoints
{
    public static void MapShortenEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/Shorten", async (
            ShortenService service,
            [FromQuery(Name = "long_url")] string longUrl,
            CancellationToken cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(longUrl))
            {
                return Results.BadRequest("The URL must be provided.");
            }

            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out Uri uriResult) ||
                (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                return Results.BadRequest("Invalid URL format. Ensure it starts with http:// or https://");
            }

            var shortenCode = await service.GetShortenUrlAsync(longUrl, cancellationToken);
            return shortenCode.IsError ? Results.Problem(shortenCode.FirstError.Description) : Results.Ok(shortenCode.Value);
        });
    }
}