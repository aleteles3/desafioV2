using System;
using System.Collections.Generic;
using Domain.Produto.Entities;
using FluentValidation;

namespace Domain.Domain.Produto.Entities
{
    public sealed class Category : Entity<Category>
    {
        //Properties
        public string Name { get; private set; }

        //Navigation Properties
        public IEnumerable<Product> Products { get; }

        //Constructors
        public Category(string name)
        {
            Name = name;
        }

        public Category(Guid id, string name) : this(name)
        {
            Id = id;
        }
        
        //Public Setters
        public void SetName(string name) => Name = name; 

        //Validations
        public override bool IsValid()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name must be informed.");
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .WithMessage("Category name must be, at least, 3 characters long.");

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }
    }
}