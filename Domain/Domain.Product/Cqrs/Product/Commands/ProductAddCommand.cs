using MediatR;

namespace Domain.Product.Cqrs.Product.Commands
{
    public class ProductAddCommand : ProductBaseCommand, IRequest<Guid?>
    {
        public ProductAddCommand(string name, string description, decimal price, int stock, Guid categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
        }
    }
}