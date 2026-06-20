using Mansari.Store.Ordering.Domain.Entities;

namespace Mansari.Store.Ordering.Domain.Orders.Interfaces;

public interface IOrderRepository
{
    Task<object?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<object>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        object order,
        CancellationToken cancellationToken = default);

    void Update(object order);

    void Remove(object order);

    Task<bool> ExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}

