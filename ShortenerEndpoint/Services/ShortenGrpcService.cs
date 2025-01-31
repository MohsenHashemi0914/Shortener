using Grpc.Core;
using Shortener.GrpcServices;

namespace ShortenerEndpoint.Services;

public sealed class ShortenGrpcService(ShortenService service) : ShortenUrl.ShortenUrlBase
{
    public async override Task<ShortenUrlResponse> GetShortenUrlAsync(ShortenUrlRequest request, ServerCallContext context)
    {
        var shortenUrl = await service.GetShortenUrlAsync(request.LongUrl, context.CancellationToken);
        return new()
        {
            ShortenUrl = shortenUrl.Value
        };
    }
}