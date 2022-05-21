﻿namespace ProjectX.Core;

public class ResultOf<TResult>
{
    public static readonly ResultOf<Unit> Unit = new(MediatR.Unit.Value);

    public TResult? Data { get; set; }

    public Error? Error { get; }

    public bool IsFailed => Error is not null;

    public ResultOf(Error error) 
        => Error = error.ThrowIfNull(); 

    public ResultOf(TResult data) 
        => Data = data.ThrowIfNull();

    public override string ToString()
    {
        var result = $"Response: {IsFailed}.";

        if (Error != null) result += $" {Error}";

        return result;
    }

    public static implicit operator ResultOf<TResult>(Error error) => new(error);

    public static implicit operator ResultOf<TResult>(TResult value) => new(value);
}