using System.Text.Json.Serialization;

namespace ProjectX.Core;

public class Error
{
    public Error(ErrorType type, ErrorCode code, string? message)
    {
        Type = type;
        Code = code;
        Message = message ?? code.GetDescription();
    }

    public Error(Exception exception)
        : this(ErrorType.ServerError, ErrorCode.ServerError, exception.Message)
    {
        Exception = exception.ThrowIfNull();
    }

    public string Message { get; }

    public ErrorCode Code { get; }

    public ErrorType Type { get; }

    [JsonIgnore]
    public Exception? Exception { get; }

    public static Error From(Exception exception)
        => new(exception);
        
    public static Error ServerError(ErrorCode code = ErrorCode.ServerError, string? message = null)
        => new(ErrorType.ServerError, code, message);

    public static Error NotFound(ErrorCode code = ErrorCode.NotFound, string? message = null)
        => new(ErrorType.NotFound, code, message);

    public static Error InvalidData(ErrorCode code = ErrorCode.InvalidData, string? message = null)
        => new(ErrorType.InvalidData, code, message);

    public static Error InvalidPermission(ErrorCode code = ErrorCode.InvalidPermission, string? message = null)
        => new(ErrorType.InvalidPermission, code, message);

    public static Error InvalidOperation(ErrorCode code = ErrorCode.InvalidOperation, string? message = null)
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