namespace ProjectX.Core.Errors;

public class Error
{
    public Error(string? message, ErrorCode code, ErrorType type, Exception? exception)
    {
        Message = message ?? code.GetDescription();
        Code = code;
        Type = type;
        Exception = exception;
    }

    public Error(ErrorType type, ErrorCode code, string? message)
        : this(message, code, type, null)
    {
    }

    public Error(Exception exception)
        : this(exception.Message, ErrorCode.ServerError, ErrorType.ServerError, exception)
    {
    }

    public string Message { get; }

    public ErrorCode Code { get; }

    public ErrorType Type { get; }

    public Exception? Exception { get; }

    public static Error From(Exception exception)
        => new(exception);
        
    public static Error ServerError(ErrorCode code, string? message = null)
        => new(ErrorType.ServerError, code, message);

    public static Error ServerError(string? message = null)
        => new(ErrorType.ServerError, ErrorCode.ServerError, message);

    public static Error NotFound(ErrorCode code, string? message = null)
        => new(ErrorType.NotFound, code, message);

    public static Error InvalidData(ErrorCode code, string? message = null)
        => new(ErrorType.InvalidData, code, message);

    public static Error InvalidPermission(ErrorCode code, string? message = null)
        => new(ErrorType.InvalidPermission, code, message);

    public override string ToString()
    {
        var result = $"Error: Type - {Type}, Code - {Code}, Message - {Message}.";

        if (Exception != null) 
        {
            result = $"{result} Exception: {Exception.GetType().Name}";
        }

        return result;
    }
}