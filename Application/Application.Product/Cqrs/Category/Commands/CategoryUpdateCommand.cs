using MediatR;

namespace Application.Product.Cqrs.Category.Commands;

public class CategoryUpdateCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}