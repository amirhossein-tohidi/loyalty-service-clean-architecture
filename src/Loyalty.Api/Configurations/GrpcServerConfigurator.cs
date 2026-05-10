using Loyalty.Api.Grpc;

namespace Loyalty.Api.Configurations;

public static class GrpcServerConfigurator
{
    public static void GrpcServerConfig(this WebApplication app)
    {
        app.MapGrpcService<LoyaltyGrpcService>();
    }
}