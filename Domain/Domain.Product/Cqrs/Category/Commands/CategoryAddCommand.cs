using MediatR;

namespace Domain.Product.Cqrs.Category.Commands;

public class CategoryAddCommand : IRequest<Guid?>
{
    public string Name { get; set; }

    public CategoryAddCommand(string name)
    {
        Name = name;
    }
}