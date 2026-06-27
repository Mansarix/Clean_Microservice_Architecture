namespace Mansari.Store.Gateway.Extensions;

public sealed class GatewayResult<T>
{
    public bool IsSuccess { get; init; }

    public T? Data { get; init; }

    public string? Error { get; init; }

    public int StatusCode { get; init; }
}