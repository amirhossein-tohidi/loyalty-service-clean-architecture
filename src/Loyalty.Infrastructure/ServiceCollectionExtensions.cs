using Loyalty.Application.Interfaces;
using Loyalty.Domain.Interfaces;
using Loyalty.Domain.Services;
using Loyalty.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Loyalty.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IFileStorageService, FileStorageService>();
        services.AddScoped<ILoyaltyCalculatorService, LoyaltyCalculatorService>();
    }
}