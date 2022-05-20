namespace ProjectX.Core;

public class PaginatedResponse<T> : Response<T>
{
    public int Total { get; }

    public PaginatedResponse(T data, int total)
        : base(data)
    {
        Total = total;
    }

    public PaginatedResponse(Error error) 
        : base(error)
    {
    }
}
