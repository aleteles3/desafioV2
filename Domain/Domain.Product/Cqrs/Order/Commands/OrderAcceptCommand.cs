using MediatR;

namespace Domain.Product.Cqrs.Order.Commands;

public class OrderAcceptCommand : IRequest
{
    public Guid OrderId { get; }

    public OrderAcceptCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}