using System;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.Produto.Entities
{
    public abstract class Entity<T> : AbstractValidator<T>
    {
        public Guid Id { get; protected set; }
        public ValidationResult ValidationResult { get; protected set; }

        protected Entity()
        {
            ValidationResult = new ValidationResult();
        }

        public abstract bool IsValid();
    }
}