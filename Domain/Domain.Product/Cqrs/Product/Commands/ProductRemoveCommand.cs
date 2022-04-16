using MediatR;

namespace Domain.Product.Cqrs.Product.Commands;

public class ProductRemoveCommand : IRequest
{
    public Guid Id { get; set; }

    public ProductRemoveCommand(Guid id)
    {
        Id = id;
    }
}