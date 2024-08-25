namespace Library.Core.DTO;

public class ResponseDto<T>(string message, bool isSuccess = true, string details = "Операция выполнена успешно", T? data = default)
{
    /// <summary>
    /// Сообщение ответа.
    /// </summary>
    public string Message { get; set; } = message;

    /// <summary>
    /// Указывает, успешен ли запрос.
    /// </summary>
    public bool IsSuccess { get; set; } = isSuccess;

    /// <summary>
    /// Данные ответа.
    /// </summary>
    public T? Data { get; set; } = data;

    /// <summary>
    /// Дополнительная информация.
    /// </summary>
    public string? Details { get; set; } = details;
}