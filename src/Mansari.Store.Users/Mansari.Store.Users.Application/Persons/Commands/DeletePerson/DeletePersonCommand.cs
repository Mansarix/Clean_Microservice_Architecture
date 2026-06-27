using MediatR;

namespace Mansari.Store.Users.Application.Persons.Commands.DeletePerson;

public sealed record DeletePersonCommand(Guid Id)
    : IRequest<bool>;
