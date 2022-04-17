using MediatR;

namespace Domain.Product.Cqrs.Category.Commands;

public class CategoryUpdateCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public CategoryUpdateCommand(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}