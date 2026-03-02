namespace ManagerSystem.Api.Common;

public class BaseResponse<T>
{
    public T? Data { get; init; }
    public List<string> Errors { get; init; } = [];
    public bool HasErrors => Errors.Count > 0;

    public static BaseResponse<T> Success(T data) => new() { Data = data };
    public static BaseResponse<T> Failure(params string[] errors) => new() { Errors = [.. errors] };
}
