namespace ProjectX.Core;

public class Response<TResult>
{
    public static readonly Response<Unit> Unit = new(MediatR.Unit.Value);

    public TResult? Data { get; set; }

    public Error? Error { get; }

    public bool IsSuccess => Error is null;

    public Response(Error error) 
        => Error = error.ThrowIfNull(); 

    public Response(TResult data) 
        => Data = data.ThrowIfNull();

    public override string ToString()
    {
        var result = $"Response: {IsSuccess}.";

        if (Error != null) result += $" {Error}";

        return result;
    }

    public static implicit operator Response<TResult>(Error error) => new(error);

    public static implicit operator Response<TResult>(TResult value) => new(value);
}