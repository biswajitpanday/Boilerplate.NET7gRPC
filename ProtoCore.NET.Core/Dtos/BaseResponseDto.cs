namespace ProtoCore.NET.Core.Dtos;

public class BaseResponseDto<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}