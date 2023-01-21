using System.Diagnostics.CodeAnalysis;

namespace ProjectX.Core;

public class ResultOf<TResult> : IResult
{
    public static readonly ResultOf<Unit> Unit = new(MediatR.Unit.Value);

    public TResult? Data { get; }

    public ApplicationError? Error { get; }

    public bool IsFailed => Error is not null;

    public ResultOf([NotNull] ApplicationError error) 
        => Error = error.ThrowIfNull(); 

    public ResultOf([NotNull] TResult data) 
        => Data = data.ThrowIfNull();

    public override string ToString()
    {
        var result = $"Response: {IsFailed}.";

        if (Error != null) result += $" {Error}";

        return result;
    }

    public static implicit operator ResultOf<TResult>(ApplicationError error) => new(error);

    public static implicit operator ResultOf<TResult>(TResult value) => new(value);
}

