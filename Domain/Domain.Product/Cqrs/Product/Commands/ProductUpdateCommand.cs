using MediatR;

namespace Domain.Product.Cqrs.Product.Commands;

public class ProductUpdateCommand : ProductBaseCommand, IRequest
{
    public Guid Id { get; set; }

    public ProductUpdateCommand(Guid id, string name, string description, decimal price, int stock, Guid categoryId)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
    }
}