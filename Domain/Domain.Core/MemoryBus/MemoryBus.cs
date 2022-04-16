using Domain.Core.Enums;
using Domain.Core.Interfaces;

namespace Domain.Core.MemoryBus;

public class MemoryBus : IMemoryBus
{
    private IList<ValidationError> ValidationErrors { get; set; }

    public IList<ValidationError> GetValidationErrors()
    {
        ValidationErrors ??= new List<ValidationError>();
        return ValidationErrors;
    }

    public void RaiseValidationError(ErrorCode errorCode, string message)
    {
        ValidationErrors ??= new List<ValidationError>();
        ValidationErrors.Add(new ValidationError(errorCode, message));
    }
}