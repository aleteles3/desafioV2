using MediatR;

namespace Application.Product.Cqrs.Category.Commands;

public class CategoryRemoveCommand : IRequest
{
    public Guid Id { get; set; }
}