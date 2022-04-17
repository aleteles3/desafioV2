using Domain.Core.Enums;
using Domain.Core.Interfaces;
using FluentValidation.Results;

namespace Domain.Core.Commands;

public class CommandHandler
{
    private IMemoryBus _memoryBus;

    protected CommandHandler(IMemoryBus memoryBus)
    {
        _memoryBus = memoryBus;
    }
    
    protected void NotifyValidationErrors(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            _memoryBus.RaiseValidationError(ErrorCode.DomainValidationError, error.ErrorMessage);
    }

    protected void NotifyValidationErrors(string error)
    {
        _memoryBus.RaiseValidationError(ErrorCode.DomainValidationError, error);
    }

    protected bool HasValidationErrors()
    {
        return _memoryBus.HasValidationErrors();
    }
}