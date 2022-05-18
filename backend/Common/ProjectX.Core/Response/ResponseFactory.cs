namespace ProjectX.Core.Response;

public static class ResponseFactory
{
    public static Response<T> Success<T>(T data)
       => new(data);

    public static Response<T> Failed<T>(Error error)
       => new(error);

    public static Response<T> Failed<T>(Exception error)
        => Failed<T>(Error.From(error));

    public static Response<T> ServerError<T>(ErrorCode code, string? message = null)
        => Failed<T>(Error.ServerError(code, message));

    public static Response<T> ServerError<T>(string message)
        => Failed<T>(Error.ServerError(message));

    public static Response<T> NotFound<T>(ErrorCode code, string? message = null)
        => Failed<T>(Error.NotFound(code, message));

    public static Response<T> InvalidData<T>(ErrorCode code, string? message = null)
        => Failed<T>(Error.InvalidData(code, message));

    public static Response<T> InvalidPermission<T>(ErrorCode code, string? message = null)
        => Failed<T>(Error.InvalidPermission(code, message));
}
