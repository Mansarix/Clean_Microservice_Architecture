using Mansari.Store.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mansari.Store.Users.Infrastructure.Persistence;

public sealed class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> People => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
