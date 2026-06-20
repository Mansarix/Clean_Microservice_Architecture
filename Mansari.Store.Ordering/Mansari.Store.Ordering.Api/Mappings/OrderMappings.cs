using Mansari.Store.Ordering.Api.Contracts;
using Mansari.Store.Ordering.Application.Orders.Commands.CreateOrder;

namespace Mansari.Store.Ordering.Api.Mappings;

public static class OrderMappings
{
    public static CreateOrderCommand ToCommand(this CreateOrderRequest request)
        => new CreateOrderCommand(request.BookId, request.Quantity);

    public static CreateOrderResponse ToResponse(this CreateOrderResponse)
        => new();
}