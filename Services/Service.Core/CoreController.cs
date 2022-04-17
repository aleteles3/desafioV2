using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.Core;

public class CoreController : Controller
{
    protected IMemoryBus _memoryBus;

    public CoreController(IMemoryBus memoryBus)
    {
        _memoryBus = memoryBus;
    }

    protected new IActionResult Response(int successStatusCode = StatusCodes.Status200OK, 
        int failureStatusCode = StatusCodes.Status400BadRequest)
    {
        return Response<object>(null, successStatusCode, failureStatusCode);
    }
    
    protected new IActionResult Response<T>(T data, int successStatusCode = StatusCodes.Status200OK, 
        int failureStatusCode = StatusCodes.Status400BadRequest)
    {
        var validationErrors = _memoryBus.GetValidationErrors();

        if (!validationErrors.Any())
            return new ObjectResult(new ResponseModelSuccess<T>(data))
            {
                StatusCode = successStatusCode
            };

        return new ObjectResult(new ResponseModelFailure(validationErrors))
        {
            StatusCode = failureStatusCode
        };
    }
}