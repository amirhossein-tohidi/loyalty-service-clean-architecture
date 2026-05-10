namespace Loyalty.Application.DTOs;

public class LoyaltyRequestDto
{
    public decimal PurchaseAmount { get; set; }
    public int CustomerType { get; set; }
    public List<decimal> LastMonthPurchases { get; set; } = [];
}