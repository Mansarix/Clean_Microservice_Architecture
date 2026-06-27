using Mansari.Store.Users.Application.Persons.DTOs;
using MediatR;

namespace Mansari.Store.Users.Application.Persons.Queries.GetPersonById;

public sealed record GetPersonByIdQuery(Guid PersonId)
    : IRequest<PersonDto?>;
