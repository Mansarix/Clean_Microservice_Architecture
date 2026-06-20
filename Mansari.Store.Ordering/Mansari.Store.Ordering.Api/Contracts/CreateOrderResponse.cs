namespace Mansari.Store.Ordering.Api.Contracts;

public sealed record CreateOrderResponse
{
    public Guid OrderId { get; init; }
    public Guid BookId { get; init; }
    public int Quantity { get; init; }
    public string Status { get; init; } = default!;
    public DateTime CreatedAtUtc { get; init; }
}