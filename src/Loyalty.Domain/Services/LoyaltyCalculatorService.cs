using Loyalty.Domain.Enums;
using Loyalty.Domain.Helpers;
using Loyalty.Domain.Interfaces;
using Loyalty.Domain.Models;
using Loyalty.Domain.Results;

namespace Loyalty.Domain.Services;

public class LoyaltyCalculatorService : ILoyaltyCalculatorService
{
    public LoyaltyResult CalculateFinalScore(LoyaltyCalculationInput input)
    {
        var purchaseAmount = input.PurchaseAmount.Value;

        var baseScore = input.CustomerType switch
        {
            CustomerType.Bronze => purchaseAmount / 100m,
            CustomerType.Silver => purchaseAmount / 80m,
            CustomerType.Gold => purchaseAmount / 60m,
            _ => throw new ArgumentOutOfRangeException(nameof(input.CustomerType))
        };

        var bonus = 0m;

        if (input.LastMonthPurchases.Count >= 5)
            bonus += 10m;

        if (purchaseAmount >= 10_000_000m)
        {
            bonus += 5m;

            var extra = purchaseAmount - 10_000_000m;
            var steps = (int)(extra / 10_000_000m);

            bonus += steps * 2.5m;
        }

        var finalScore = baseScore + baseScore.Percent(bonus);

        return new LoyaltyResult(finalScore);
    }
}