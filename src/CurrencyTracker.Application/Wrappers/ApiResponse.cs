namespace CurrencyTracker.Application.Wrappers;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }



    public static ApiResponse<T> Success(T data, string? message = null)
    {
        return new ApiResponse<T> { Data = data, IsSuccess = true, Message = message };
    }

    public static ApiResponse<T> Success(string message)

    {
        return new ApiResponse<T> { IsSuccess = true, Message = message };
    }

    public static ApiResponse<T> Fail(string error, string? message = null)
    {
        return new ApiResponse<T> { IsSuccess = false, Message = message, Errors = new List<string> { error } };
    }

    public static ApiResponse<T> Fail(List<string> errors, string? message = null)
    {
        return new ApiResponse<T>{ IsSuccess = false, Message = message, Errors = errors };
    }


}
