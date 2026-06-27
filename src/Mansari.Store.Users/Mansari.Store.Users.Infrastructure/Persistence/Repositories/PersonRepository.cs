using Mansari.Store.Users.Domain.Entities;
using Mansari.Store.Users.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mansari.Store.Users.Infrastructure.Persistence.Repositories;

public sealed class PersonRepository : IPersonRepository
{
    private readonly UsersDbContext _dbContext;

    public PersonRepository(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Person?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.People
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Person>> GetPagedAsync(
        string? search,
        bool? isActive,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilters(search, isActive);

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .ThenByDescending(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        string? search,
        bool? isActive,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilters(search, isActive);

        return await query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(
        Person person,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.People.AddAsync(person, cancellationToken);
    }

    public void Update(Person person)
    {
        _dbContext.People.Update(person);
    }

    public async Task<bool> ExistsByNationalIdAsync(
        string nationalId,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedNationalId = NormalizeDigits(nationalId);

        var query = _dbContext.People.AsNoTracking()
            .Where(x => x.NationalId.Value == normalizedNationalId);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByMobileNumberAsync(
        string mobileNumber,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedMobileNumber = NormalizeDigits(mobileNumber);

        var query = _dbContext.People.AsNoTracking()
            .Where(x => x.MobileNumber.Value == normalizedMobileNumber);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(
        string username,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();

        var query = _dbContext.People.AsNoTracking()
            .Where(x => x.Username.Value == normalizedUsername);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        var query = _dbContext.People.AsNoTracking()
            .Where(x => x.Email.Value == normalizedEmail);

        if (excludeId.HasValue)
            query = query.Where(x => x.Id != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    private static string NormalizeDigits(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var builder = new System.Text.StringBuilder(value.Length);

        foreach (var ch in value.Trim())
        {
            builder.Append(ch switch
            {
                '۰' => '0',
                '۱' => '1',
                '۲' => '2',
                '۳' => '3',
                '۴' => '4',
                '۵' => '5',
                '۶' => '6',
                '۷' => '7',
                '۸' => '8',
                '۹' => '9',
                '٠' => '0',
                '١' => '1',
                '٢' => '2',
                '٣' => '3',
                '٤' => '4',
                '٥' => '5',
                '٦' => '6',
                '٧' => '7',
                '٨' => '8',
                '٩' => '9',
                _ => ch
            });
        }

        return builder.ToString();
    }

    private IQueryable<Person> ApplyFilters(
        string? search,
        bool? isActive)
    {
        var query = _dbContext.People
            .AsNoTracking()
            .Where(x => x.DeletedAtUtc == null);

        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim().ToLowerInvariant();

            query = query.Where(x =>
                x.FirstName.Value.ToLower().Contains(search) ||
                x.LastName.Value.ToLower().Contains(search) ||
                x.NationalId.Value.Contains(search) ||
                x.MobileNumber.Value.Contains(search) ||
                x.Username.Value.Contains(search) ||
                x.Email.Value.ToLower().Contains(search));
        }

        return query;
    }
}
