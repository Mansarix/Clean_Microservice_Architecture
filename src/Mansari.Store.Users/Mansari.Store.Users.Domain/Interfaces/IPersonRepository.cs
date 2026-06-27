using Mansari.Store.Users.Domain.Entities;

namespace Mansari.Store.Users.Domain.Interfaces;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Person>> GetPagedAsync(
        string? search,
        bool? isActive,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        string? search,
        bool? isActive,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Person person,
        CancellationToken cancellationToken = default);

    void Update(Person person);

    Task<bool> ExistsByNationalIdAsync(
        string nationalId,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByMobileNumberAsync(
        string mobileNumber,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByUsernameAsync(
        string username,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(
        string email,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default);
}
