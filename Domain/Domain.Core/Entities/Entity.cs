using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Domain.Core.Entities
{
    public abstract class Entity<T> : AbstractValidator<T>
    {
        public Guid Id { get; protected set; }
        [Required]
        public DateTimeOffset DateInc { get; protected set; }
        [Required]
        public DateTimeOffset DateAlter { get; protected set; }
        [NotMapped]
        public ValidationResult ValidationResult { get; protected set; }

        protected Entity()
        {
            ValidationResult = new ValidationResult();
        }

        public abstract bool IsValid();

        public void SetDateInc(DateTimeOffset dateInc) => DateInc = dateInc;
        public void SetDateAlter(DateTimeOffset dateAlter) => DateAlter = dateAlter;
    }
}