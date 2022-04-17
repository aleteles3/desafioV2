using System.Collections;

namespace Service.Core;

public class ResponseModelSuccess<T>
{
    public int DataCount => GetDataCount(Data);
    public T Data { get; set; }

    public ResponseModelSuccess(T data)
    {
        Data = data;
    }
    
    private int GetDataCount<TR>(TR data)
    {
        if (data is ICollection collection)
            return collection.Count;

        return data != null ? 1 : 0;
    }
}