using Loyalty.Application.Interfaces;
using Loyalty.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Loyalty.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ILoyaltyService, LoyaltyService>();
    }
}