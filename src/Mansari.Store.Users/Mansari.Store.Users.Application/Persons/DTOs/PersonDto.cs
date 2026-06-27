namespace Mansari.Store.Users.Application.Persons.DTOs;

public sealed record PersonDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string NationalId,
    string MobileNumber,
    string Username,
    string Email,
    DateOnly BirthDate,
    string Gender,
    bool IsActive,
    string? Notes,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    DateTime? DeletedAtUtc
);
