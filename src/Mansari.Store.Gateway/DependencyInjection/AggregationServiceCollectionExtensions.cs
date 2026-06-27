using Microsoft.Extensions.DependencyInjection;

namespace Mansari.Store.Gateway.DependencyInjection;

public static class AggregationServiceCollectionExtensions
{
    public static IServiceCollection AddAggregationServices(
        this IServiceCollection services)
    {
        return services;
    }
}