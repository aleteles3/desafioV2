using Domain.Core.Enums;

namespace Domain.Core.MemoryBus;

public class ValidationError
{
    public ErrorCode ErrorCode { get; }
    public string Message { get; }

    public ValidationError(ErrorCode errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }
}