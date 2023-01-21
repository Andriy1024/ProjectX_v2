namespace ProjectX.Core;

public interface IResult
{
    ApplicationError? Error { get; }

    bool IsFailed { get; }
}