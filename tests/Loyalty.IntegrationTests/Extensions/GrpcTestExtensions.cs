using Grpc.Net.Client;
using Loyalty.IntegrationTests.Fixtures;

namespace Loyalty.IntegrationTests.Extensions;

public static class GrpcFactoryExtensions
{
    public static TClient CreateGrpcClient<TClient>(
        this ApiWebApplicationFactory factory,
        Func<GrpcChannel, TClient> creator)
        where TClient : class
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(creator);

        var httpClient = factory.CreateDefaultClient();

        var channel = GrpcChannel.ForAddress(httpClient.BaseAddress!, new GrpcChannelOptions
        {
            HttpClient = httpClient
        });

        return creator(channel);
    }
}