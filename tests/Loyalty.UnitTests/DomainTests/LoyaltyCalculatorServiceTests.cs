using Loyalty.Domain.Enums;
using Loyalty.Domain.Models;
using Loyalty.Domain.Services;
using Loyalty.Domain.ValueObjects;
using Xunit;

namespace Loyalty.UnitTests.DomainTests;

public class LoyaltyCalculatorServiceTests
{
    private readonly LoyaltyCalculatorService _service = new();

    [Fact]
    public void CalculateFinalScore_Bronze_NoBonus_ShouldReturnCorrectScore()
    {
        var input = new LoyaltyCalculationInput(
            PurchaseValue.From(1_000_000m),
            CustomerType.Bronze,
            [PurchaseValue.From(100m), PurchaseValue.From(200m)]
        );

        var result = _service.CalculateFinalScore(input);

        Assert.Equal(10_000m, result.FinalScore);
    }

    [Fact]
    public void CalculateFinalScore_Silver_NoBonus_ShouldReturnCorrectScore()
    {
        var input = new LoyaltyCalculationInput(
            PurchaseValue.From(800_000m),
            CustomerType.Silver,
            [PurchaseValue.From(100m)]
        );

        var result = _service.CalculateFinalScore(input);

        Assert.Equal(10_000m, result.FinalScore);
    }

    [Fact]
    public void CalculateFinalScore_Gold_NoBonus_ShouldReturnCorrectScore()
    {
        var input = new LoyaltyCalculationInput(
            PurchaseValue.From(600_000m),
            CustomerType.Gold,
            [PurchaseValue.From(100m)]
        );

        var result = _service.CalculateFinalScore(input);

        Assert.Equal(10_000m, result.FinalScore);
    }

    [Fact]
    public void CalculateFinalScore_WithFivePurchases_ShouldAddTenPercentBonus()
    {
        var input = new LoyaltyCalculationInput(
            PurchaseValue.From(1_000_000m),
            CustomerType.Bronze,
            [
                PurchaseValue.From(100),
                PurchaseValue.From(200),
                PurchaseValue.From(300),
                PurchaseValue.From(400),
                PurchaseValue.From(500)
            ]
        );

        var result = _service.CalculateFinalScore(input);

        Assert.Equal(11_000m, result.FinalScore);
    }

    [Fact]
    public void CalculateFinalScore_WhenPurchaseAbove10M_ShouldAddFivePercentBonus()
    {
        var input = new LoyaltyCalculationInput(
            PurchaseValue.From(10_000_000m),
            CustomerType.Bronze,
            [PurchaseValue.From(100)]
        );

        var result = _service.CalculateFinalScore(input);

        Assert.Equal(105_000m, result.FinalScore);
    }

    [Fact]
    public void CalculateFinalScore_WhenPurchaseAbove20M_ShouldAddStepBonus()
    {
        var input = new LoyaltyCalculationInput(
            PurchaseValue.From(20_000_000m),
            CustomerType.Bronze,
            [PurchaseValue.From(100)]
        );

        var result = _service.CalculateFinalScore(input);

        Assert.Equal(215_000m, result.FinalScore);
    }

    [Fact]
    public void CalculateFinalScore_WhenAllBonusesApplied_ShouldReturnCorrectScore()
    {
        var input = new LoyaltyCalculationInput(
            PurchaseValue.From(15_000_000m),
            CustomerType.Gold,
            [
                PurchaseValue.From(2_000_000),
                PurchaseValue.From(3_000_000),
                PurchaseValue.From(1_500_000),
                PurchaseValue.From(4_000_000),
                PurchaseValue.From(1_000_000)
            ]
        );

        var result = _service.CalculateFinalScore(input);

        Assert.Equal(287_500m, result.FinalScore);
    }

    [Fact]
    public void CalculateFinalScore_InvalidCustomerType_ShouldThrowException()
    {
        var input = new LoyaltyCalculationInput(
            PurchaseValue.From(1_000_000m),
            (CustomerType)999,
            [PurchaseValue.From(100)]
        );

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _service.CalculateFinalScore(input));
    }
}