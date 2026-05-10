using Scalar.AspNetCore;

namespace Loyalty.Api.Configurations;

public static class ScalarConfiguration
{
    public static void ConfigureScalar(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options => { options.Title = "Loyalty API"; });
        }
        else
        {
            app.UseHsts();
        }
    }
}