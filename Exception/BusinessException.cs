using bank.Entity;

namespace bank.Exceptions;

public class BusinessException(List<ApiError> errors) : Exception
{
    public List<ApiError> Errors { get; } = errors;
}
