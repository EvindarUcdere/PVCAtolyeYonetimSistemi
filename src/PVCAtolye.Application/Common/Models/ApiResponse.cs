namespace PVCAtolye.Application.Common.Models;

public sealed record ApiResponse<T>(bool Success, string Message, T? Data, IReadOnlyCollection<string> Errors);

public static class ApiResponse
{
    public static ApiResponse<T> Ok<T>(T data, string message = "Islem basarili.") => new(true, message, data, Array.Empty<string>());

    public static ApiResponse<T> Fail<T>(string message, params string[] errors) => new(false, message, default, errors);
}
