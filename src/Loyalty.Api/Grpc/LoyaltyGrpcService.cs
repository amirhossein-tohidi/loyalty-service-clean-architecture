using Grpc.Core;
using Loyalty.Application.DTOs;
using Loyalty.Application.Interfaces;
using Loyalty.GrpcContracts;

namespace Loyalty.Api.Grpc;

public class LoyaltyGrpcService(ILoyaltyService service) : LoyaltyGrpc.LoyaltyGrpcBase
{
    public override Task<LoyaltyResponse> Calculate(LoyaltyRequest request, ServerCallContext context)
    {
        var dto = new LoyaltyRequestDto
        {
            PurchaseAmount = (decimal)request.PurchaseAmount,
            CustomerType = request.CustomerType,
            LastMonthPurchases = request.LastMonthPurchases?
                .Select(x => (decimal)x)
                .ToList() ?? []
        };

        var result = service.Calculate(dto);

        return Task.FromResult(new LoyaltyResponse
        {
            FinalScore = (double)result.FinalScore
        });
    }
}