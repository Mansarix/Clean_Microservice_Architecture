using Mansari.Store.Users.Application.Persons.DTOs;
using Mansari.Store.Users.Application.Persons.Mapping;
using Mansari.Store.Users.Domain.Common;
using Mansari.Store.Users.Domain.Entities;
using Mansari.Store.Users.Domain.Interfaces;
using MediatR;

namespace Mansari.Store.Users.Application.Persons.Commands.CreatePerson;

public sealed class CreatePersonCommandHandler
    : IRequestHandler<CreatePersonCommand, PersonDto>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePersonCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PersonDto> Handle(
        CreatePersonCommand request,
        CancellationToken cancellationToken)
    {
        if (await _personRepository.ExistsByNationalIdAsync(request.NationalId, cancellationToken: cancellationToken))
            throw new DomainException("A person with the same national id already exists.");

        if (await _personRepository.ExistsByMobileNumberAsync(request.MobileNumber, cancellationToken: cancellationToken))
            throw new DomainException("A person with the same mobile number already exists.");

        if (await _personRepository.ExistsByUsernameAsync(request.Username, cancellationToken: cancellationToken))
            throw new DomainException("A person with the same username already exists.");

        if (await _personRepository.ExistsByEmailAsync(request.Email, cancellationToken: cancellationToken))
            throw new DomainException("A person with the same email already exists.");

        var person = Person.Create(
            request.FirstName,
            request.LastName,
            request.NationalId,
            request.MobileNumber,
            request.Username,
            request.Email,
            request.BirthDate,
            request.Gender,
            request.Notes,
            request.IsActive);

        await _personRepository.AddAsync(person, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PersonMapper.ToDto(person);
    }
}
