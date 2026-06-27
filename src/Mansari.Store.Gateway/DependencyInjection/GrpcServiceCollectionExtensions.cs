using Mansari.Store.Gateway.Configuration;

namespace Mansari.Store.Gateway.DependencyInjection;

public static class GrpcServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<GrpcOptions>(
    configuration.GetSection("Grpc"));

        return services;
    }
}