namespace Domain.Product.Cqrs.Order.Commands;

public class OrderItemAddCommand
{
    public Guid ProductId { get; set; }
    public decimal ListPrice { get; set; }
    public decimal Discount { get; set; }
    public int Quantity { get; set; }
}