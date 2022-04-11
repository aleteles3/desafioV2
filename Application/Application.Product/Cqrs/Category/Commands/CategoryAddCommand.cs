using MediatR;

namespace Application.Product.Cqrs.Category.Commands;

public class CategoryAddCommand : IRequest<Guid?>
{
    public string Name { get; set; }
}