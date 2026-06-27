using Microsoft.Extensions.DependencyInjection;

namespace Mansari.Store.Gateway.DependencyInjection;

public static class GatewayServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayServices(
        this IServiceCollection services)
    {
        services.AddControllers();

        return services;
    }
}