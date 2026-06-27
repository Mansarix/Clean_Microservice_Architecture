using Mansari.Store.Users.Domain.Interfaces;

namespace Mansari.Store.Users.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly UsersDbContext _dbContext;

    public UnitOfWork(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
