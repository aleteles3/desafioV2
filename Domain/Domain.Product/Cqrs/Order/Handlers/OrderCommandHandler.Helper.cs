using System.Text;
using OrderDomain = Domain.Product.Entities.Order;
using ProductDomain = Domain.Product.Entities.Product;

namespace Domain.Product.Cqrs.Order.Handlers;

public partial class OrderCommandHandler
{
    private async Task CheckOrderItemsAvailability(OrderDomain order, IEnumerable<ProductDomain> products)
    {
        var productIds = order.OrderItems.Select(x => x.ProductId);
        
        var outOfStockItems = order.OrderItems
            .Select(oi => products.FirstOrDefault(p => p.Id == oi.ProductId && p.Stock < oi.Quantity))
            .Where(outOfStock => outOfStock != null)
            .ToList();

        if (outOfStockItems.Any())
        {
            var stringBuilder = new StringBuilder("The following Products are out of stock:");
            foreach (var outOfStockItem in outOfStockItems)
                stringBuilder.AppendLine($"Id: {outOfStockItem.Id}, Name: {outOfStockItem.Name}");
            
            NotifyValidationErrors(stringBuilder.ToString());
        }
        
        var notFoundProductsIds = productIds.Except(products.Select(x => x.Id)).ToList();

        if (notFoundProductsIds.Any())
        {
            var stringBuilder = new StringBuilder("The following Products were not found:");
            foreach (var notFoundProductId in notFoundProductsIds)
                stringBuilder.AppendLine($"{notFoundProductId},");
            
            NotifyValidationErrors(stringBuilder.ToString());
        }
    }

    private async Task ReserveOrderItems(OrderDomain order, IEnumerable<ProductDomain> products)
    {
        foreach (var orderItem in order.OrderItems)
        {
            var product = products.First(x => x.Id == orderItem.ProductId);
            product.AddStock(-orderItem.Quantity);
            
            await _productRepository.UpdateProductAsync(product);
        }
    }
}