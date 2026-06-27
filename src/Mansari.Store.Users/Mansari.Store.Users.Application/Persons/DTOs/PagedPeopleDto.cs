namespace Mansari.Store.Users.Application.Persons.DTOs;

public sealed record PagedPeopleDto(
    IReadOnlyList<PersonDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount
);
