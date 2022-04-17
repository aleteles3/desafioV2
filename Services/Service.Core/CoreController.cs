using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Service.Core;

public class CoreController : Controller
{
    protected IMemoryBus _memoryBus;

    public CoreController(IMemoryBus memoryBus)
    {
        _memoryBus = memoryBus;
    }

    protected new IActionResult Response()
    {
        var validationErrors = _memoryBus.GetValidationErrors();

        if (!validationErrors.Any())
            return Ok(new ResponseModel<object>(true, DateTimeOffset.Now, null, validationErrors));

        return BadRequest(new ResponseModel<object>(false, DateTimeOffset.Now, null, validationErrors));
    }
    
    protected new IActionResult Response<T>(T data)
    {
        var validationErrors = _memoryBus.GetValidationErrors();

        if (!validationErrors.Any())
            return Ok(new ResponseModel<T>(true, DateTimeOffset.Now, data, validationErrors));

        var instanceToReturn = GetInstanceToReturn<T>();
        
        return BadRequest(new ResponseModel<T>(false, DateTimeOffset.Now, instanceToReturn, validationErrors));
    }
    
    /// <summary>
    /// Produces the default return value for type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private dynamic GetInstanceToReturn<T>()
    {
        dynamic instance;
        var type = typeof(T);
        if (type.IsArray)
            instance = Array.CreateInstance(type.GetElementType() ?? typeof(object), 0);
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            var elementType = type.GetGenericArguments()[0];
            var listType = typeof(List<>).MakeGenericType(elementType);
            instance = Activator.CreateInstance(listType);
        }
        else if (type.GetConstructor(Type.EmptyTypes) != null || type.IsValueType)
            instance = default(T);
        else
            instance = null;

        return instance;
    }
}