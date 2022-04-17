using Domain.Core.MemoryBus;

namespace Service.Core;

public class ResponseModelFailure
{
    public IEnumerable<ValidationError> ValidationErrors { get; set; }

    public ResponseModelFailure(IEnumerable<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }
}