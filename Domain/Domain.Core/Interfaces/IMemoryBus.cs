using Domain.Core.Enums;
using Domain.Core.MemoryBus;

namespace Domain.Core.Interfaces;

public interface IMemoryBus
{
    IList<ValidationError> GetValidationErrors();
    void RaiseValidationError(ErrorCode errorCode, string message);
}