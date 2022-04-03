using System;
using Domain.Produto.Entities;
using FluentValidation;

namespace Domain.Domain.Produto.Entities
{
    public sealed class Product : Entity<Product>
    {
        //Properties
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Stock { get; private set; }

        //Navigation Properties
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        
        //Constructors
        public Product(string name, string description, decimal price, int stock, Guid categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
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
        public void SetPrice(decimal price) => Price = price;
        public void SetStock(int stock) => Stock = stock;
        
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
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than 0");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product stock cannot be less than 0.");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }
}