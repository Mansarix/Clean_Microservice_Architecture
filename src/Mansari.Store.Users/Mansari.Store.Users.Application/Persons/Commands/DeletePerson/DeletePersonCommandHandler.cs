using Mansari.Store.Users.Domain.Interfaces;
using MediatR;

namespace Mansari.Store.Users.Application.Persons.Commands.DeletePerson;

public sealed class DeletePersonCommandHandler
    : IRequestHandler<DeletePersonCommand, bool>
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonCommandHandler(
        IPersonRepository personRepository,
        IUnitOfWork unitOfWork)
    {
        _personRepository = personRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        DeletePersonCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _personRepository
            .GetByIdAsync(request.Id, cancellationToken);

        if (person is null || person.IsDeleted)
            return false;

        person.Delete();

        _personRepository.Update(person);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
