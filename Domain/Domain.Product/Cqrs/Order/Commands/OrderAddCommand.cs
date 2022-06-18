using MediatR;

namespace Domain.Product.Cqrs.Order.Commands;

public sealed class OrderAddCommand : IRequest<Guid?>
{
    public Guid UserId { get; }
    public IEnumerable<OrderItemAddCommand> OrderItemAddCommands { get; }

    public OrderAddCommand(Guid userId, IEnumerable<OrderItemAddCommand> orderItemAddCommands)
    {
        UserId = userId;
        OrderItemAddCommands = orderItemAddCommands;
    }
}