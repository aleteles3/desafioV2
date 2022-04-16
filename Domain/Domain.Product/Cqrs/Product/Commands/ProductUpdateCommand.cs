using MediatR;

namespace Domain.Product.Cqrs.Product.Commands;

public class ProductUpdateCommand : ProductBaseCommand, IRequest
{
    public Guid Id { get; set; }
}