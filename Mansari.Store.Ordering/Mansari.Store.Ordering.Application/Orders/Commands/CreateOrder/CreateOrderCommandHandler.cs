using Mansari.Store.Ordering.Application.Common;
using Mansari.Store.Ordering.Application.DTOs;
using Mansari.Store.Ordering.Domain.Abstractions;
using Mansari.Store.Ordering.Domain.Orders.Entities;
using Mansari.Store.Ordering.Domain.Orders.Enums;
using Mansari.Store.Ordering.Domain.Orders.Interfaces;
using MediatR;

namespace Mansari.Store.Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Result<CreateOrderResponse>>
{
    private readonly IOrderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IOrderRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateOrderResponse>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = Order.Create(
                request.BookId,
                request.Quantity);

            await _repository.AddAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<CreateOrderResponse>.Success(new CreateOrderResponse(order.Id, order.Quantity,order.Status == OrderStatus.Confirmed, order.FailureReason));
        }
        catch (Exception ex)
        {
            return Result<CreateOrderResponse>.Failure(ex.Message);
        }
    }
}
