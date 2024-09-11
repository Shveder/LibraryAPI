namespace Library.Application.DTO;

public class ResponseDto<T>(string message, bool isSuccess = true, string details = "Operation succeed", T? data = default)
{
    /// <summary>
    /// Answer message.
    /// </summary>
    public string Message { get; set; } = message;

    /// <summary>
    /// Is the request successful.
    /// </summary>
    public bool IsSuccess { get; set; } = isSuccess;

    /// <summary>
    /// Answer data.
    /// </summary>
    public T? Data { get; set; } = data;

    /// <summary>
    /// Additional information.
    /// </summary>
    public string? Details { get; set; } = details;
}