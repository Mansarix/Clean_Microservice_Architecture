using Google.Protobuf.WellKnownTypes;
using Mansari.Store.Users.Api.Extensions;
using Mansari.Store.Users.Api.Protos;
using Mansari.Store.Users.Application.Persons.Commands.CreatePerson;
using Mansari.Store.Users.Application.Persons.Commands.DeletePerson;
using Mansari.Store.Users.Application.Persons.Commands.UpdatePerson;
using Mansari.Store.Users.Application.Persons.DTOs;
using Mansari.Store.Users.Application.Persons.Queries.GetPeople;
using Mansari.Store.Users.Application.Persons.Queries.GetPersonById;
using Mansari.Store.Users.Domain.Common;
using Mansari.Store.Users.Domain.Enums;
using MediatR;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Mansari.Store.Users.Api.Services;

public sealed class UsersGrpcService : Users.UsersBase
{
    private readonly IMediator _mediator;

    public UsersGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<UserResponse> GetUserById(
        GetUserByIdRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "User id is invalid."));

        var person = await _mediator.Send(
            new GetPersonByIdQuery(id),
            context.CancellationToken);

        if (person is null)
            throw new RpcException(new Status(StatusCode.NotFound, "User not found."));

        return ToGrpcResponse(person);
    }

    public override async Task<UsersResponse> GetUsers(
        GetUsersRequest request,
        ServerCallContext context)
    {
        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 20 : Math.Min(request.PageSize, 100);

        bool? onlyActive = request.HasOnlyActive ? request.OnlyActive : null;

        var result = await _mediator.Send(
            new GetPeopleQuery(request.Search, pageNumber, pageSize, onlyActive),
            context.CancellationToken);

        var response = new UsersResponse
        {
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };

        response.Items.AddRange(result.Items.Select(ToGrpcResponse));

        return response;
    }

    public override async Task<UserResponse> CreateUser(
        CreateUserRequest request,
        ServerCallContext context)
    {
        try
        {
            var command = new CreatePersonCommand(
                request.FirstName,
                request.LastName,
                request.NationalId,
                request.MobileNumber,
                request.Username,
                request.Email,
                request.BirthDate.ToDateOnly(),
                (PersonGender)request.Gender,
                request.Notes,
                request.HasIsActive ? request.IsActive : null);

            var person = await _mediator.Send(command, context.CancellationToken);

            return ToGrpcResponse(person);
        }
        catch (Exception ex)
        {
            throw MapException(ex);
        }
    }

    public override async Task<UserResponse> UpdateUser(
        UpdateUserRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "User id is invalid."));

        try
        {
            var command = new UpdatePersonCommand(
                id,
                request.FirstName,
                request.LastName,
                request.NationalId,
                request.MobileNumber,
                request.Username,
                request.Email,
                request.BirthDate.ToDateOnly(),
                (PersonGender)request.Gender,
                request.Notes,
                request.IsActive);

            var person = await _mediator.Send(command, context.CancellationToken);

            if (person is null)
                throw new RpcException(new Status(StatusCode.NotFound, "User not found."));

            return ToGrpcResponse(person);
        }
        catch (Exception ex)
        {
            throw MapException(ex);
        }
    }

    public override async Task<DeleteUserResponse> DeleteUser(
        DeleteUserRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "User id is invalid."));

        try
        {
            var success = await _mediator.Send(
                new DeletePersonCommand(id),
                context.CancellationToken);

            if (!success)
                throw new RpcException(new Status(StatusCode.NotFound, "User not found."));

            return new DeleteUserResponse { Success = true };
        }
        catch (Exception ex)
        {
            throw MapException(ex);
        }
    }

    private static UserResponse ToGrpcResponse(PersonDto dto)
    {
        var response = new UserResponse
        {
            Id = dto.Id.ToString(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            FullName = dto.FullName,
            NationalId = dto.NationalId,
            MobileNumber = dto.MobileNumber,
            Username = dto.Username,
            Email = dto.Email,
            BirthDate = dto.BirthDate.ToTimestamp(),
            Gender = (Gender)dto.Gender,
            IsActive = dto.IsActive,
            Notes = dto.Notes ?? string.Empty,
            CreatedAtUtc = dto.CreatedAtUtc.ToTimestamp()
        };

        if (dto.UpdatedAtUtc.HasValue)
            response.UpdatedAtUtc = dto.UpdatedAtUtc.Value.ToTimestamp();

        if (dto.DeletedAtUtc.HasValue)
            response.DeletedAtUtc = dto.DeletedAtUtc.Value.ToTimestamp();

        return response;
    }

    private static RpcException MapException(Exception ex)
    {
        if (ex is RpcException rpcException)
            return rpcException;

        if (ex is DomainException domainException)
        {
            var statusCode = domainException.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase)
                ? StatusCode.AlreadyExists
                : StatusCode.InvalidArgument;

            return new RpcException(new Status(statusCode, domainException.Message));
        }

        if (ex is DbUpdateException)
            return new RpcException(new Status(StatusCode.Internal, "Database update failed."));

        if (ex is OperationCanceledException)
            return new RpcException(new Status(StatusCode.Cancelled, "Request cancelled."));

        return new RpcException(new Status(StatusCode.Internal, "Unexpected server error."));
    }
}
