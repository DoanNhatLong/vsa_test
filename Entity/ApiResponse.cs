namespace bank.Entity;

public record ApiError(string Field, string Message);

public record PaginationInfo(
    int Page,
    int PageSize,
    int TotalPages,
    int TotalItems,
    bool HasNext,
    bool HasPrevious
);

public record ApiResponse<T>
{
    public required bool Success { get; init; }
    public required int StatusCode { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }
    public List<ApiError>? Errors { get; init; }
    public PaginationInfo? Pagination { get; init; }

    public static ApiResponse<T> SuccessResponse(T data, string msg = "Success", PaginationInfo? pagination = null)
        => new() { Success = true, StatusCode = 200, Message = msg, Data = data, Pagination = pagination };

    public static ApiResponse<T> ErrorResponse(int statusCode, string message, List<ApiError>? errors = null)
        => new() { Success = false, StatusCode = statusCode, Message = message, Errors = errors };
}
