using FluentValidation;
using Loyalty.Application.DTOs;
using Loyalty.Application.Extensions;
using Loyalty.Application.Validators;
using Loyalty.Domain.Enums;
using Xunit;

namespace Loyalty.UnitTests.ApplicationTests;

public class LoyaltyRequestValidatorTests
{
    [Fact]
    public void Validate_NullDto_ShouldThrowValidationException()
    {
        var ex = Assert.Throws<ValidationException>(() =>
            LoyaltyRequestValidator.Validate(null!));

        Assert.Equal("Request cannot be null", ex.Message);
    }

    [Fact]
    public void Validate_NegativePurchaseAmount_ShouldThrowValidationException()
    {
        var dto = new LoyaltyRequestDto
        {
            PurchaseAmount = -1,
            CustomerType = CustomerType.Bronze.ToInt(),
            LastMonthPurchases = [100, 200]
        };

        var ex = Assert.Throws<ValidationException>(() =>
            LoyaltyRequestValidator.Validate(dto));

        Assert.Equal("PurchaseAmount must be greater than zero", ex.Message);
    }

    [Fact]
    public void Validate_InvalidCustomerType_ShouldThrowValidationException()
    {
        var dto = new LoyaltyRequestDto
        {
            PurchaseAmount = 100,
            CustomerType = 999,
            LastMonthPurchases = [100, 200]
        };

        var ex = Assert.Throws<ValidationException>(() =>
            LoyaltyRequestValidator.Validate(dto));

        Assert.Equal("CustomerType is invalid", ex.Message);
    }

    [Fact]
    public void Validate_NullLastMonthPurchases_ShouldThrowValidationException()
    {
        var dto = new LoyaltyRequestDto
        {
            PurchaseAmount = 100,
            CustomerType = CustomerType.Bronze.ToInt(),
            LastMonthPurchases = null!
        };

        var ex = Assert.Throws<ValidationException>(() =>
            LoyaltyRequestValidator.Validate(dto));

        Assert.Equal("LastMonthPurchases cannot be null", ex.Message);
    }

    [Fact]
    public void Validate_NegativeValueInLastMonthPurchases_ShouldThrowValidationException()
    {
        var dto = new LoyaltyRequestDto
        {
            PurchaseAmount = 100,
            CustomerType = CustomerType.Bronze.ToInt(),
            LastMonthPurchases = [100, -1, 200]
        };

        var ex = Assert.Throws<ValidationException>(() =>
            LoyaltyRequestValidator.Validate(dto));

        Assert.Equal("LastMonthPurchases contains negative value", ex.Message);
    }

    [Fact]
    public void Validate_ValidDto_ShouldNotThrow()
    {
        var dto = new LoyaltyRequestDto
        {
            PurchaseAmount = 100,
            CustomerType = CustomerType.Bronze.ToInt(),
            LastMonthPurchases = [100, 200]
        };

        var exception = Record.Exception(() => LoyaltyRequestValidator.Validate(dto));

        Assert.Null(exception);
    }
}