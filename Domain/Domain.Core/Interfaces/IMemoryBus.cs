using Domain.Core.Enums;
using Domain.Core.MemoryBus;

namespace Domain.Core.Interfaces;

public interface IMemoryBus
{
    bool HasValidationErrors();
    IList<ValidationError> GetValidationErrors();
    void RaiseValidationError(ErrorCode errorCode, string message);
}