using AutoMapper;
using Domain.Product.Cqrs.Order.Commands;
using Domain.Product.Entities;

namespace Application.Product.AutoMapper;

public class CommandToDomainMappingProfile : Profile
{
    public CommandToDomainMappingProfile()
    {
        CreateMap<OrderAddCommand, Order>()
            .ConstructUsing((src, _) =>
            {
                var order = new Order(Guid.NewGuid(), src.UserId);
                var orderItems = src.OrderItemAddCommands.Select(x => new OrderItem(Guid.NewGuid(),
                    order.Id, x.ProductId, x.ListPrice, x.Discount, x.Quantity));
                order.OrderItems = orderItems;

                return order;
            });
    }
}