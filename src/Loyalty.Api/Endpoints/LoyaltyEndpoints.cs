using Loyalty.Application.DTOs;
using Loyalty.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Loyalty.Api.Endpoints;

public static class LoyaltyEndpoints
{
    public static void Map(WebApplication app)
    {
        var map = app.MapGroup("/api/v1/loyalty").WithTags("Loyalty");

        map.MapPost("/calculate", ([FromBody] LoyaltyRequestDto dto, ILoyaltyService service) =>
        Results.Ok(service.Calculate(dto)));
    }
}