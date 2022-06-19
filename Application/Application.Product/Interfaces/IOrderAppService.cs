using Application.Product.ViewModels.Crud;

namespace Application.Product.Interfaces;

public interface IOrderAppService
{
    Task AddOrder(AddOrderViewModel addOrderViewModel);
    Task AcceptOrder(Guid orderId);
}