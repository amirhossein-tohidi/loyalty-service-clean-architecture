using FluentValidation;
using Loyalty.Application.DTOs;
using Loyalty.Application.Extensions;
using Loyalty.Application.Interfaces;
using Loyalty.Application.Services;
using Loyalty.Domain.Enums;
using Loyalty.Domain.Interfaces;
using Loyalty.Domain.Models;
using Loyalty.Domain.Results;
using Moq;
using Xunit;

namespace Loyalty.UnitTests.ApplicationTests;

public class LoyaltyServiceTests
{
    private readonly Mock<ILoyaltyCalculatorService> _calculator = new();
    private readonly Mock<IFileStorageService> _fileStorage = new();

    private LoyaltyService CreateService() => new(_calculator.Object, _fileStorage.Object);

    [Fact]
    public void Calculate_ShouldThrow_WhenDtoIsInvalid()
    {
        var service = CreateService();

        var invalid = new LoyaltyRequestDto
        {
            CustomerType = CustomerType.Bronze.ToInt(),
            PurchaseAmount = -10,
            LastMonthPurchases = []
        };

        Assert.Throws<ValidationException>(() => service.Calculate(invalid));
    }

    [Fact]
    public void Calculate_ShouldCallCalculator_AndReturnResponse()
    {
        var service = CreateService();

        var dto = new LoyaltyRequestDto
        {
            CustomerType = CustomerType.Gold.ToInt(),
            PurchaseAmount = 100000,
            LastMonthPurchases = [50000m, 25000m]
        };

        var expectedScore = new LoyaltyResult(250);

        _calculator
            .Setup(x => x.CalculateFinalScore(It.IsAny<LoyaltyCalculationInput>()))
            .Returns(expectedScore);

        var result = service.Calculate(dto);

        Assert.Equal(expectedScore.FinalScore, result.FinalScore);

        _calculator.Verify(x => x.CalculateFinalScore(It.IsAny<LoyaltyCalculationInput>()), Times.Once);

        _fileStorage.Verify(x => x.Save(It.IsAny<object>()), Times.Once);
    }
}