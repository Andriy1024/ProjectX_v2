using System.Text.Json.Serialization;

namespace ProjectX.Core;

public class ApplicationError
{
    public ApplicationError(ErrorType type, ErrorCode code, string? message)
    {
        Type = type;
        Code = code;
        Message = message ?? code.GetDescription();
    }

    public ApplicationError(Exception exception)
        : this(ErrorType.ServerError, ErrorCode.ServerError, exception.Message)
    {
        Exception = exception.ThrowIfNull();
    }

    public string Message { get; }

    public ErrorCode Code { get; }

    public ErrorType Type { get; }

    [JsonIgnore]
    public Exception? Exception { get; }

    public static ApplicationError From(Exception exception)
        => new(exception);
        
    public static ApplicationError ServerError(ErrorCode code = ErrorCode.ServerError, string? message = null)
        => new(ErrorType.ServerError, code, message);

    public static ApplicationError NotFound(ErrorCode code = ErrorCode.NotFound, string? message = null)
        => new(ErrorType.NotFound, code, message);

    public static ApplicationError InvalidData(ErrorCode code = ErrorCode.BadData, string? message = null)
        => new(ErrorType.BadData, code, message);

    public static ApplicationError InvalidPermission(ErrorCode code = ErrorCode.InvalidPermission, string? message = null)
        => new(ErrorType.InvalidPermission, code, message);

    public static ApplicationError InvalidOperation(ErrorCode code = ErrorCode.InvalidOperation, string? message = null)
        => new(ErrorType.InvalidOperation, code, message);

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