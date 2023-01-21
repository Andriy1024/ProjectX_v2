namespace ProjectX.Core.Errors.Exceptions;

public class BadDataException : ApplicationException
{
    public BadDataException()
        : this(ErrorCode.BadData)
    {
    }

    public BadDataException(ApplicationError error)
        : base(error)
    {
    }

    public BadDataException(ErrorCode code, string? message = null)
        : base(ErrorType.BadData, code, message)
    {
    }
}