namespace ProjectX.Core;

public interface IMaybeFailed
{
    Error? Error { get; }

    bool IsFailed { get; }
}