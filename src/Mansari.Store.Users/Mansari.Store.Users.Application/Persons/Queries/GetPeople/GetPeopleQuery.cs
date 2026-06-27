using Mansari.Store.Users.Application.Persons.DTOs;
using MediatR;

namespace Mansari.Store.Users.Application.Persons.Queries.GetPeople;

public sealed record GetPeopleQuery(
    string? Search = null,
    int PageNumber = 1,
    int PageSize = 20,
    bool? IsActive = null
) : IRequest<PagedPeopleDto>;
