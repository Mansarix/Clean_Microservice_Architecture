namespace Mansari.Store.Gateway.Configuration;

public sealed class GrpcOptions
{
    public required ServiceEndpoint Catalog { get; init; }

    public required ServiceEndpoint Basket { get; init; }

    public required ServiceEndpoint Ordering { get; init; }

    public required ServiceEndpoint User { get; init; }
}

public sealed class ServiceEndpoint
{
    public required string Address { get; init; }
}
