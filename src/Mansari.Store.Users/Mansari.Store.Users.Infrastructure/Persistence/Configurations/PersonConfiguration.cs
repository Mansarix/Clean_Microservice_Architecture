using Mansari.Store.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mansari.Store.Users.Infrastructure.Persistence.Configurations;

public sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("people");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc);

        builder.Property(x => x.DeletedAtUtc);

        builder.Property(x => x.BirthDate)
            .IsRequired();

        builder.Property(x => x.Gender)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.OwnsOne(x => x.FirstName, firstName =>
        {
            firstName.Property(x => x.Value)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(x => x.LastName, lastName =>
        {
            lastName.Property(x => x.Value)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(x => x.NationalId, nationalId =>
        {
            nationalId.Property(x => x.Value)
                .HasColumnName("national_id")
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.OwnsOne(x => x.MobileNumber, mobileNumber =>
        {
            mobileNumber.Property(x => x.Value)
                .HasColumnName("mobile_number")
                .HasMaxLength(11)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Username, username =>
        {
            username.Property(x => x.Value)
                .HasColumnName("username")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.OwnsOne(x => x.Email, email =>
        {
            email.Property(x => x.Value)
                .HasColumnName("email")
                .HasMaxLength(254)
                .IsRequired();
        });

        builder.Navigation(x => x.FirstName).IsRequired();
        builder.Navigation(x => x.LastName).IsRequired();
        builder.Navigation(x => x.NationalId).IsRequired();
        builder.Navigation(x => x.MobileNumber).IsRequired();
        builder.Navigation(x => x.Username).IsRequired();
        builder.Navigation(x => x.Email).IsRequired();

        builder.HasIndex("FirstName_Value");
        builder.HasIndex("LastName_Value");
        builder.HasIndex("NationalId_Value").IsUnique();
        builder.HasIndex("MobileNumber_Value").IsUnique();
        builder.HasIndex("Username_Value").IsUnique();
        builder.HasIndex("Email_Value").IsUnique();
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.CreatedAtUtc);
    }
}
