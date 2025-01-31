using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Shortener.GrpcServices.Extensions;

public static class GrpcExtensions
{
    public static IServiceCollection AddShortenerGrpc(this IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
        });
        
        return services;
    }

    public static IServiceCollection AddShortenerGrpcClient<TClient>(this IServiceCollection services) where TClient : class
    {
        services.AddGrpcClient<TClient>(options =>
        {
            options.Address = new("https://localhost:7056");
        });

        return services;
    }

    public static GrpcServiceEndpointConventionBuilder MapShortenerGrpcService<TService>(this IEndpointRouteBuilder app) where TService : class
    {
        return app.MapGrpcService<TService>();
    }
}