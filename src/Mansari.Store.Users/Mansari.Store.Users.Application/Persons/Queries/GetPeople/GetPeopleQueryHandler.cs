using Mansari.Store.Users.Application.Persons.DTOs;
using Mansari.Store.Users.Application.Persons.Mapping;
using Mansari.Store.Users.Domain.Interfaces;
using MediatR;

namespace Mansari.Store.Users.Application.Persons.Queries.GetPeople;

public sealed class GetPeopleQueryHandler
    : IRequestHandler<GetPeopleQuery, PagedPeopleDto>
{
    private readonly IPersonRepository _personRepository;

    public GetPeopleQueryHandler(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<PagedPeopleDto> Handle(
        GetPeopleQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 20 : Math.Min(request.PageSize, 100);

        var totalCount = await _personRepository.CountAsync(
            request.Search,
            request.IsActive,
            cancellationToken);

        var persons = await _personRepository.GetPagedAsync(
            request.Search,
            request.IsActive,
            pageNumber,
            pageSize,
            cancellationToken);

        var items = persons
            .Select(PersonMapper.ToDto)
            .ToList();

        return new PagedPeopleDto(items, pageNumber, pageSize, totalCount);
    }
}
