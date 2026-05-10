using Loyalty.Api.Interceptors;
using Loyalty.Api.Middlewares;
using Loyalty.Application;
using Loyalty.Infrastructure;

namespace Loyalty.Api.Configurations;

public static class ServiceConfigurator
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddGrpc(options => { options.Interceptors.Add<GlobalGrpcExceptionInterceptor>(); });

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices();

        builder.Services.AddExceptionHandler<GlobalExceptionMiddleware>();
        builder.Services.AddProblemDetails();
    }
}