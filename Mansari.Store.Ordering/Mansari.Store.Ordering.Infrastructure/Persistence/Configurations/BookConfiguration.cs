using Mansari.Store.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Mansari.Store.Catalog.Infrastructure.Persistence.Configurations;

public sealed class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc);

        builder.OwnsOne(x => x.Title, title =>
        {
            title.Property(t => t.Value)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Author, author =>
        {
            author.Property(a => a.Value)
                .HasColumnName("author")
                .HasMaxLength(150)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Price, price =>
        {
            price.Property(p => p.Amount)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            price.Property(p => p.Currency)
                .HasColumnName("currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Stock, stock =>
        {
            stock.Property(s => s.Value)
                .HasColumnName("stock")
                .IsRequired();
        });

        builder.Navigation(x => x.Title).IsRequired();
        builder.Navigation(x => x.Author).IsRequired();
        builder.Navigation(x => x.Price).IsRequired();
        builder.Navigation(x => x.Stock).IsRequired();
    }
}
