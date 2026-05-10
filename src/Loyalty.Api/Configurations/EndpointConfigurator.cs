using Loyalty.Api.Endpoints;

namespace Loyalty.Api.Configurations;

public static class EndpointConfigurator
{
    public static void ConfigureEndpoints(this WebApplication app)
    {
        LoyaltyEndpoints.Map(app);

        app.MapGet("/", () => "Loyalty Api" + DateTime.Now);

        app.MapGet("/env", () => app.Environment.EnvironmentName);

        app.MapGet("/ver", () => "Version 1.0.0");
    }
}