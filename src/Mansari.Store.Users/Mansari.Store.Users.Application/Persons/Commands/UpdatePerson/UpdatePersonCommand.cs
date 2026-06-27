using Mansari.Store.Users.Application.Persons.DTOs;
using Mansari.Store.Users.Domain.Enums;
using MediatR;

namespace Mansari.Store.Users.Application.Persons.Commands.UpdatePerson;

public sealed record UpdatePersonCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalId,
    string MobileNumber,
    string Username,
    string Email,
    DateOnly BirthDate,
    PersonGender Gender = PersonGender.Unknown,
    string? Notes = null,
    bool IsActive = true
) : IRequest<PersonDto?>;
