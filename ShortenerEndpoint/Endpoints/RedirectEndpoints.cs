using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ShortenerEndpoint.Services;

namespace ShortenerEndpoint.Endpoints;

public static class RedirectEndpoints
{
    public static void MapRedirectEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/{shorten_code:required}", async (
            ShortenService service,
            [FromRoute(Name = "shorten_code")] string shortenCode,
            CancellationToken cancellationToken) =>
        {
            var destinationUrl = await service.GetDestinationUrlAsync(shortenCode, cancellationToken);
            if (destinationUrl.IsError)
            {
                return Results.Problem(destinationUrl.FirstError.Description);
            }

            return Results.Redirect(destinationUrl.Value);
        });
    }
}