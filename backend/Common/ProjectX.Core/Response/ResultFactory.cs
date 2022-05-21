namespace ProjectX.Core;

public static class ResultFactory
{
    public static ResultOf<T> Success<T>(T data)
        => new(data);

    public static PaginatedResultOf<T> Success<T>(T data, int total) 
        => new PaginatedResultOf<T>(data, total);

    public static ResultOf<T> Failed<T>(Error error)
        => new(error);

    public static ResultOf<T> Failed<T>(Exception error)
        => Failed<T>(Error.From(error));

    public static ResultOf<T> ServerError<T>(ErrorCode code, string? message = null)
        => Failed<T>(Error.ServerError(code, message));

    public static ResultOf<T> ServerError<T>(string message)
        => Failed<T>(Error.ServerError(message));

    public static ResultOf<T> NotFound<T>(ErrorCode code, string? message = null)
        => Failed<T>(Error.NotFound(code, message));

    public static ResultOf<T> InvalidData<T>(ErrorCode code, string? message = null)
        => Failed<T>(Error.InvalidData(code, message));

    public static ResultOf<T> InvalidPermission<T>(ErrorCode code, string? message = null)
        => Failed<T>(Error.InvalidPermission(code, message));
}
