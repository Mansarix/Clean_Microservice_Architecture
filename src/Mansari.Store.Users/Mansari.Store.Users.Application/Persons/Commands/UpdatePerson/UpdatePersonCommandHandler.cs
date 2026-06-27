using Mansari.Store.Users.Application.Persons.DTOs;
using Mansari.Store.Users.Application.Persons.Mapping;
using Mansari.Store.Users.Domain.Common;
using Mansari.Store.Users.Domain.Interfaces;
using MediatR;

namespace Mansari.Store.Users.Application.Persons.Commands.UpdatePerson;

public sealed class UpdatePersonCommandHandler
    : IRequestHandler<UpdatePersonCommand, PersonDto?>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePersonCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PersonDto?> Handle(
        UpdatePersonCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository
            .GetByIdAsync(request.Id, cancellationToken);

        if (person is null || person.IsDeleted)
            return null;

        if (await _personRepository.ExistsByNationalIdAsync(request.NationalId, request.Id, cancellationToken))
            throw new DomainException("Another person with the same national id already exists.");

        if (await _personRepository.ExistsByMobileNumberAsync(request.MobileNumber, request.Id, cancellationToken))
            throw new DomainException("Another person with the same mobile number already exists.");

        if (await _personRepository.ExistsByUsernameAsync(request.Username, request.Id, cancellationToken))
            throw new DomainException("Another person with the same username already exists.");

        if (await _personRepository.ExistsByEmailAsync(request.Email, request.Id, cancellationToken))
            throw new DomainException("Another person with the same email already exists.");

        person.UpdateDetails(
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

        _personRepository.Update(person);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PersonMapper.ToDto(person);
    }
}
