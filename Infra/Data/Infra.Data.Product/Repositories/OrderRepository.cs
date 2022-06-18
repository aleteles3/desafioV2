using Domain.Product.Entities;
using Domain.Product.Interfaces;
using Infra.Data.Core;
using Infra.Data.Product.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Product.Repositories;

public class OrderRepository : CoreRepository, IOrderRepository
{
    public OrderRepository(ProductContext productContext) : base(productContext) { }

    public async Task<Order> GetOrderByIdAsync(Guid id)
    {
        return await Context.Set<Order>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddOrderAsync(Order order)
    {
        await AddAsync(order);
        await SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        Update(order);
        await SaveChangesAsync();
    }
}