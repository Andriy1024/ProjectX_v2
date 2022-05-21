namespace ProjectX.Core;

public class PaginatedResultOf<T> : ResultOf<T>
{
    public int Total { get; }

    public PaginatedResultOf(T data, int total)
        : base(data)
    {
        Total = total;
    }

    public PaginatedResultOf(Error error) 
        : base(error)
    {
    }
}