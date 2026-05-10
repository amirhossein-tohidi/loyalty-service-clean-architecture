using Loyalty.Application.DTOs;
using Loyalty.Domain.Enums;
using Loyalty.Domain.Models;
using Loyalty.Domain.Results;
using Loyalty.Domain.ValueObjects;

namespace Loyalty.Application.Mapping;

public static class LoyaltyMapper
{
    public static LoyaltyCalculationInput ToDomain(this LoyaltyRequestDto dto)
    {
        return new LoyaltyCalculationInput(
            purchaseAmount: PurchaseValue.From(dto.PurchaseAmount),
            customerType: dto.ToCustomerType(),
            lastMonthPurchases: dto.LastMonthPurchases
                .Select(PurchaseValue.From)
                .ToList()
        );
    }

    private static CustomerType ToCustomerType(this LoyaltyRequestDto dto) => (CustomerType)dto.CustomerType;

    public static LoyaltyResponseDto ToDto(this LoyaltyResult domain)
    {
        return new LoyaltyResponseDto
        {
            FinalScore = domain.FinalScore
        };
    }
}