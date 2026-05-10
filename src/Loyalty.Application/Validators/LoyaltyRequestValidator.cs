using FluentValidation;
using Loyalty.Application.DTOs;
using Loyalty.Domain.Enums;

namespace Loyalty.Application.Validators;

public static class LoyaltyRequestValidator
{
    public static void Validate(LoyaltyRequestDto dto)
    {
        if (dto == null)
            throw new ValidationException("Request cannot be null");

        if (dto.PurchaseAmount <= 0)
            throw new ValidationException("PurchaseAmount must be greater than zero");

        if (!Enum.IsDefined(typeof(CustomerType), dto.CustomerType))
            throw new ValidationException("CustomerType is invalid");

        if (dto.LastMonthPurchases == null)
            throw new ValidationException("LastMonthPurchases cannot be null");

        if (dto.LastMonthPurchases.Any(x => x < 0))
            throw new ValidationException("LastMonthPurchases contains negative value");
    }
}