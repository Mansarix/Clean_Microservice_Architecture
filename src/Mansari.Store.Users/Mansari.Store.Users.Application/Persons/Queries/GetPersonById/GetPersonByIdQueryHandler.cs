using Mansari.Store.Users.Application.Persons.DTOs;
using Mansari.Store.Users.Application.Persons.Mapping;
using Mansari.Store.Users.Domain.Interfaces;
using MediatR;

namespace Mansari.Store.Users.Application.Persons.Queries.GetPersonById;

public sealed class GetPersonByIdQueryHandler
    : IRequestHandler<GetPersonByIdQuery, PersonDto?>
{
    private readonly IPersonRepository _personRepository;

    public GetPersonByIdQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PersonDto?> Handle(
        GetPersonByIdQuery request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository
            .GetByIdAsync(request.PersonId, cancellationToken);

        if (person is null || person.IsDeleted)
            return null;

        return PersonMapper.ToDto(person);
    }
}
