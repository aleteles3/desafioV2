using Domain.Core.Interfaces;
using Domain.Product.Entities;

namespace Domain.Product.Interfaces;

public interface IOrderRepository : ICoreRepository
{
    Task<Order> GetOrderByIdAsync(Guid id);
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
}