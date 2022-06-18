using Domain.Core.Entities;
using FluentValidation;

namespace Domain.Product.Entities;

public sealed class OrderItem : Entity<OrderItem>
{
    //Properties
    public decimal ListPrice { get; }
    public decimal Discount { get; }
    
    //Navigation Properties
    public Guid OrderId { get; }
    public Order Order { get; set; }
    public Guid ProductId { get; }
    public Product Product { get; set; }
    
    //Constructors
    public OrderItem(Guid orderId, Guid productId, decimal listPrice, decimal discount)
    {
        OrderId = orderId;
        ProductId = productId;
        ListPrice = listPrice;
        Discount = discount;
    }

    public OrderItem(Guid id, Guid orderId, Guid productId, decimal listPrice, decimal discount)
        : this(orderId, productId, listPrice, discount)
    {
        Id = id;
    }

    //Validations
    public override bool IsValid()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order Id must be informed.");
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product Id must be informed.");
        RuleFor(x => x.ListPrice)
            .GreaterThan(0)
            .WithMessage("List Price must be greater than 0.");
        RuleFor(x => x.Discount)
            .InclusiveBetween(0, 1)
            .WithMessage("Discount must be between 0 and 1.");

        ValidationResult = Validate(this);

        return ValidationResult.IsValid;
    }
}