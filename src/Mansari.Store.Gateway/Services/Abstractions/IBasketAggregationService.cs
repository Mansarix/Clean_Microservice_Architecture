namespace Mansari.Store.Gateway.Services.Abstractions;

public interface IBasketAggregationService
{
    Task<IResult> GetBasketAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}