namespace ProjectX.Core.Errors.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException()
        : this(ErrorCode.NotFound)
    {
    }

    public NotFoundException(ApplicationError error)
        : base(error)
    {
    }

    public NotFoundException(ErrorCode code, string? message = null)
        : base(ErrorType.NotFound, code, message)
    {
    }
}