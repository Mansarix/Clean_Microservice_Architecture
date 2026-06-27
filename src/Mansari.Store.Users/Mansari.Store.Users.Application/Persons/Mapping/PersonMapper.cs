using Mansari.Store.Users.Application.Persons.DTOs;
using Mansari.Store.Users.Domain.Entities;

namespace Mansari.Store.Users.Application.Persons.Mapping;

internal static class PersonMapper
{
    public static PersonDto ToDto(Person person)
    {
        return new PersonDto(
            person.Id,
            person.FirstName.Value,
            person.LastName.Value,
            person.GetFullName(),
            person.NationalId.Value,
            person.MobileNumber.Value,
            person.Username.Value,
            person.Email.Value,
            person.BirthDate,
            person.Gender.ToString(),
            person.IsActive,
            person.Notes,
            person.CreatedAtUtc,
            person.UpdatedAtUtc,
            person.DeletedAtUtc);
    }
}
