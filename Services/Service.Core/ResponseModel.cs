using System.Collections;
using Domain.Core.MemoryBus;

namespace Service.Core;

public class ResponseModel<T>
{
    public bool Success { get; set; }
    public DateTimeOffset ResultDate { get; set; }
    public T Data { get; set; }
    public int DataCount => GetDataCount(Data);
    public IEnumerable<ValidationError> ValidationErrors { get; set; }

    public ResponseModel(bool success, DateTimeOffset resultDate, T data, IEnumerable<ValidationError> validationErrors)
    {
        Success = success;
        ResultDate = resultDate;
        Data = data;
        ValidationErrors = validationErrors;
    }
    
    private int GetDataCount<T>(T data)
    {
        if (data is ICollection collection)
            return collection.Count;

        return data != null ? 1 : 0;
    }
}