using MediatR;

namespace Domain.Product.Cqrs.Product.Commands
{
    public class ProductAddCommand : ProductBaseCommand, IRequest<Guid?>
    {
    
    }
}