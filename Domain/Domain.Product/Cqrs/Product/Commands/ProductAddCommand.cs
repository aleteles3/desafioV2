using MediatR;

namespace Application.Product.Cqrs.Product.Commands
{
    public class ProductAddCommand : ProductBaseCommand, IRequest<Guid?>
    {
    
    }
}