using MediatR;

namespace Domain.Product.Cqrs.Category.Commands;

public class CategoryAddCommand : IRequest<Guid?>
{
    public string Name { get; set; }
}