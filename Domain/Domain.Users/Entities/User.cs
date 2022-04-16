using Domain.Core.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Domain.User.Entities;

public sealed class User : Entity<User>
{
    //Properties
    public string Login { get; }
    public string Password { get; private set; }
    public bool Status { get; private set; }

    //Constructors
    public User(string login, string password, bool status = true)
    {
        Login = login;
        Password = password;
        Status = status;
    }

    public User(Guid id, string login, string password, bool status = true)
        : this(login, password, status)
    {
        Id = id;
    }

    //Public Setters
    public void SetStatus(bool status) => Status = status;
    public void SetPassword(string password) => Password = password;
    
    //Validations
    public override bool IsValid()
    {
        RuleFor(x => x.Login)
            .EmailAddress()
            .WithMessage("Not a valid email address.");
        RuleFor(x => x.Password)
            .Custom((password, context) =>
            {
                if (password.Length < 8)
                    context.AddFailure(new ValidationFailure(
                        nameof(Password), "The password must be, at least, 8 characters long."));
                if (!password.Any(char.IsUpper))
                    context.AddFailure(new ValidationFailure(
                        nameof(Password), "The password must contain, at least, 1 upper case character."));
                if (!password.Any(char.IsLower))
                    context.AddFailure(new ValidationFailure(
                        nameof(Password), "The password must contain, at least, 1 lower case character."));
                if (password.All(char.IsLetterOrDigit))
                    context.AddFailure(new ValidationFailure(
                        nameof(Password), "The password must contain, at least, 1 special character."));
            });

        ValidationResult = Validate(this);

        return ValidationResult.IsValid;
    }
}