using Mansari.Store.Users.Domain.Common;
using Mansari.Store.Users.Domain.Enums;
using Mansari.Store.Users.Domain.ValueObjects;

namespace Mansari.Store.Users.Domain.Entities;

public sealed class Person : Entity, IAggregateRoot
{
    public FirstName FirstName { get; private set; } = default!;
    public LastName LastName { get; private set; } = default!;
    public NationalId NationalId { get; private set; } = default!;
    public MobileNumber MobileNumber { get; private set; } = default!;
    public Username Username { get; private set; } = default!;
    public EmailAddress Email { get; private set; } = default!;

    public DateOnly BirthDate { get; private set; }
    public PersonGender Gender { get; private set; }

    public bool IsActive { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }

    public string? Notes { get; private set; }

    private Person() { }

    private Person(
        Guid id,
        FirstName firstName,
        LastName lastName,
        NationalId nationalId,
        MobileNumber mobileNumber,
        Username username,
        EmailAddress email,
        DateOnly birthDate,
        PersonGender gender,
        string? notes)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        NationalId = nationalId;
        MobileNumber = mobileNumber;
        Username = username;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        IsActive = true;
    }

    public static Person Create(
        string firstName,
        string lastName,
        string nationalId,
        string mobileNumber,
        string username,
        string email,
        DateOnly birthDate,
        PersonGender gender = PersonGender.Unknown,
        string? notes = null,
        bool? isActive = null)
    {
        ValidateBirthDate(birthDate);

        var person = new Person(
            Guid.NewGuid(),
            FirstName.Create(firstName),
            LastName.Create(lastName),
            NationalId.Create(nationalId),
            MobileNumber.Create(mobileNumber),
            Username.Create(username),
            EmailAddress.Create(email),
            birthDate,
            gender,
            notes);

        person.IsActive = isActive ?? true;

        return person;
    }

    public void UpdateDetails(
        string firstName,
        string lastName,
        string nationalId,
        string mobileNumber,
        string username,
        string email,
        DateOnly birthDate,
        PersonGender gender,
        string? notes,
        bool isActive)
    {
        EnsureNotDeleted();
        ValidateBirthDate(birthDate);

        FirstName = FirstName.Create(firstName);
        LastName = LastName.Create(lastName);
        NationalId = NationalId.Create(nationalId);
        MobileNumber = MobileNumber.Create(mobileNumber);
        Username = Username.Create(username);
        Email = EmailAddress.Create(email);
        BirthDate = birthDate;
        Gender = gender;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
        IsActive = isActive;

        MarkAsUpdated();
    }

    public void Delete()
    {
        EnsureNotDeleted();

        IsActive = false;
        DeletedAtUtc = DateTime.UtcNow;

        MarkAsUpdated();
    }

    public string GetFullName()
    {
        return $"{FirstName.Value} {LastName.Value}";
    }

    public bool IsDeleted => DeletedAtUtc.HasValue;

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Person is already deleted.");
    }

    private static void ValidateBirthDate(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (birthDate > today)
            throw new DomainException("Birth date cannot be in the future.");
    }
}
