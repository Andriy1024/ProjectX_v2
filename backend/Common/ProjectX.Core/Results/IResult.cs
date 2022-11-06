namespace ProjectX.Core;

public interface IResult
{
    Error? Error { get; }

    bool IsFailed { get; }
}