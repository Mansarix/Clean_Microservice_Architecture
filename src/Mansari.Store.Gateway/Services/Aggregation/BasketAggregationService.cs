using Mansari.Store.Gateway.Services.Abstractions;

namespace Mansari.Store.Gateway.Services.Aggregation;

public sealed class BasketAggregationService
    : IBasketAggregationService
{
    public Task<IResult> GetBasketAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
