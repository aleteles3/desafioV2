using MediatR;

namespace Domain.Product.Cqrs.Order.Commands;

public sealed class OrderAddCommand : IRequest<Guid?>
{
    public Guid UserId { get; set; }
    public IEnumerable<OrderItemAddCommand> OrderItemAddCommands { get; set; }
}