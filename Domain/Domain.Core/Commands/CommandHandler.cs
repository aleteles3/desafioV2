using FluentValidation.Results;

namespace Domain.Core.Commands;

public class CommandHandler
{
    protected void NotifyValidationErrors(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            Console.WriteLine(error);
    }

    protected void NotifyValidationErrors(string error)
    {
        Console.WriteLine(error);
    }
}