using Domain.Core.Entities;
using Domain.Core.Enums.Product;
using Domain.Product.Shared;
using FluentValidation;

namespace Domain.Product.Entities;

public sealed class Order : Entity<Order>
{
    //Properties
    public OrderStatus OrderStatus { get; private set; }
    
    //Navigation Properties
    public Guid UserId { get; }
    public User User { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
    
    //Constructors
    public Order(Guid userId)
    {
        UserId = userId;
        OrderStatus = OrderStatus.Pending;
    }

    public Order(Guid id, Guid userId) : this(userId)
    {
        Id = id;
    }
    
    //Public Setters
    public void SetOrderStatus(OrderStatus orderStatus) => OrderStatus = orderStatus;
    
    //Validations
    public override bool IsValid()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id must be informed.");

        RuleForEach(x => x.OrderItems)
            .Custom((x, context) =>
            {
                if (!x.IsValid())
                    foreach (var validationResultError in x.ValidationResult.Errors)
                        context.AddFailure(validationResultError);
            });

        ValidationResult = Validate(this);

        return ValidationResult.IsValid;
    }
}