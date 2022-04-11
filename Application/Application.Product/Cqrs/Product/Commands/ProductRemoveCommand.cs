using MediatR;

namespace Application.Product.Cqrs.Product.Commands;

public class ProductRemoveCommand : IRequest
{
    public Guid Id { get; set; }
}