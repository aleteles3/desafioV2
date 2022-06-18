using Domain.Core.Entities;
using FluentValidation;

namespace Domain.Product.Entities
{
    public sealed class Product : Entity<Product>
    {
        //Properties
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal ListPrice { get; private set; }
        public int Stock { get; private set; }

        //Navigation Properties
        public Guid CategoryId { get; private set; }
        public Category Category { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        
        //Constructors
        public Product(string name, string description, decimal listPrice, int stock, Guid categoryId)
        {
            Name = name;
            Description = description;
            ListPrice = listPrice;
            Stock = stock;
            CategoryId = categoryId;
        }

        public Product(Guid id, string name, string description, decimal price, int stock, Guid categoryId) 
            : this(name, description, price, stock, categoryId)
        {
            Id = id;
        }
        
        //Public Setters
        public void SetName(string name) => Name = name;
        public void SetDescription(string description) => Description = description;
        public void SetPrice(decimal price) => ListPrice = price;
        public void SetStock(int stock) => Stock = stock;
        public void SetCategoryId(Guid categoryId) => CategoryId = categoryId;
        
        //Validations
        public override bool IsValid()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name must be informed.");
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .WithMessage("Product name must be, at least, 3 characters long.");
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Product description must be informed.");
            RuleFor(x => x.ListPrice)
                .GreaterThan(0)
                .WithMessage("Product listPrice must be greater than 0.");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product stock cannot be less than 0.");
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Category Id must be informed.");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }
}