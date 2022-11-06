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

    public static implicit operator PaginatedResultOf<T>(Error error) => new(error);

    public static implicit operator PaginatedResultOf<T>((T Data, int Total) value) => new(value.Data, value.Total);
}