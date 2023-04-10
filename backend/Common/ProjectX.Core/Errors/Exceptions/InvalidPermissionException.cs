namespace ProjectX.Core.Errors.Exceptions;

public class InvalidPermissionException : ApplicationException
{
    public InvalidPermissionException()
        : this(ErrorCode.InvalidPermission)
    {
    }

    public InvalidPermissionException(ApplicationError error)
        : base(error)
    {
    }

    public InvalidPermissionException(ErrorCode code, string? message = null)
        : base(ErrorType.InvalidPermission, code, message)
    {
    }
}
