using Loyalty.Domain.Enums;
using Loyalty.Domain.ValueObjects;

namespace Loyalty.Domain.Models;

public class LoyaltyCalculationInput
{
    public PurchaseValue PurchaseAmount { get; }
    public CustomerType CustomerType { get; }
    public IReadOnlyList<PurchaseValue> LastMonthPurchases { get; }

    public LoyaltyCalculationInput(
        PurchaseValue purchaseAmount,
        CustomerType customerType,
        IEnumerable<PurchaseValue> lastMonthPurchases)
    {
        PurchaseAmount = purchaseAmount;
        CustomerType = customerType;
        LastMonthPurchases = lastMonthPurchases.ToList();
    }
}