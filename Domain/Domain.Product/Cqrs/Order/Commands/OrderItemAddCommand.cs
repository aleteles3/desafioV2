namespace Domain.Product.Cqrs.Order.Commands;

public class OrderItemAddCommand
{
    public Guid ProductId { get; }
    public decimal ListPrice { get; }
    public decimal Discount { get; }

    public OrderItemAddCommand(Guid productId, decimal listPrice, decimal discount)
    {
        ProductId = productId;
        ListPrice = listPrice;
        Discount = discount;
    }
}