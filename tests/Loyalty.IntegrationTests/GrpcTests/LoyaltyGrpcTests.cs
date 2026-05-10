using Grpc.Core;
using Loyalty.Domain.Enums;
using Loyalty.GrpcContracts;
using Loyalty.IntegrationTests.Extensions;
using Loyalty.IntegrationTests.Fixtures;
using Xunit;

namespace Loyalty.IntegrationTests.GrpcTests;

public class LoyaltyGrpcTests(ApiWebApplicationFactory factory)
    : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task CalculateGrpc_ShouldReturnScore()
    {
        var client = factory.CreateGrpcClient(channel => new LoyaltyGrpc.LoyaltyGrpcClient(channel));
        var request = new LoyaltyRequest
        {
            PurchaseAmount = 15_000_000,
            CustomerType = (int)CustomerType.Gold,
            LastMonthPurchases =
            {
                2_000_000,
                3_000_000,
                1_500_000,
                4_000_000,
                1_000_000
            }
        };

        var response = await client.CalculateAsync(request);

        Assert.NotNull(response);
        Assert.Equal(287500, response.FinalScore);
    }


    [Fact]
    public async Task CalculateGrpc_ShouldReturnFinalScore_AccordingToBusinessRules()
    {
        var client = factory.CreateGrpcClient(channel => new LoyaltyGrpc.LoyaltyGrpcClient(channel));

        // Gold => base = 15,000,000 / 60 = 250,000
        // bonuses = 15%
        // final = 287,500
        var request = new LoyaltyRequest
        {
            PurchaseAmount = 15_000_000,
            CustomerType = (int)CustomerType.Gold,
            LastMonthPurchases =
            {
                2_000_000,
                3_000_000,
                1_500_000,
                4_000_000,
                1_000_000
            }
        };

        var response = await client.CalculateAsync(request);

        Assert.NotNull(response);
        Assert.Equal(287500, response.FinalScore);
    }

    [Fact]
    public async Task CalculateGrpc_BronzeWithoutBonus_ShouldReturnExpectedScore()
    {
        var client = factory.CreateGrpcClient(channel => new LoyaltyGrpc.LoyaltyGrpcClient(channel));

        // Bronze => purchaseAmount / 100
        var request = new LoyaltyRequest
        {
            PurchaseAmount = 1_000_000,
            CustomerType = (int)CustomerType.Bronze,
            LastMonthPurchases =
            {
                100,
                200
            }
        };

        var response = await client.CalculateAsync(request);

        Assert.NotNull(response);
        Assert.Equal(10_000, response.FinalScore);
    }

    [Fact]
    public async Task CalculateGrpc_WhenPurchaseAmountIsZero_ShouldReturnInvalidArgument()
    {
        var client = factory.CreateGrpcClient(channel => new LoyaltyGrpc.LoyaltyGrpcClient(channel));

        var request = new LoyaltyRequest
        {
            PurchaseAmount = 0,
            CustomerType = (int)CustomerType.Bronze,
            LastMonthPurchases = { }
        };

        var ex = await Assert.ThrowsAsync<RpcException>(() => client.CalculateAsync(request).ResponseAsync);

        Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
    }

    [Fact]
    public async Task CalculateGrpc_WhenPurchaseAmountIsNegative_ShouldReturnInvalidArgument()
    {
        var client = factory.CreateGrpcClient(channel => new LoyaltyGrpc.LoyaltyGrpcClient(channel));

        var request = new LoyaltyRequest
        {
            PurchaseAmount = -1,
            CustomerType = (int)CustomerType.Bronze,
            LastMonthPurchases = { }
        };

        var ex = await Assert.ThrowsAsync<RpcException>(() => client.CalculateAsync(request).ResponseAsync);

        Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
    }

    [Fact]
    public async Task CalculateGrpc_WhenPurchasesContainNegative_ShouldReturnInvalidArgument()
    {
        var client = factory.CreateGrpcClient(channel => new LoyaltyGrpc.LoyaltyGrpcClient(channel));

        var request = new LoyaltyRequest
        {
            PurchaseAmount = 1_000_000,
            CustomerType = (int)CustomerType.Bronze,
            LastMonthPurchases =
            {
                1000,
                -500
            }
        };

        var ex = await Assert.ThrowsAsync<RpcException>(() => client.CalculateAsync(request).ResponseAsync);

        Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
    }

    [Fact]
    public async Task CalculateGrpc_ShouldReturnErrorForInvalidInput()
    {
        var client = factory.CreateGrpcClient(channel => new LoyaltyGrpc.LoyaltyGrpcClient(channel));
        var request = new LoyaltyRequest
        {
            PurchaseAmount = -100,
            CustomerType = 0,
            LastMonthPurchases = { }
        };

        var ex = await Assert.ThrowsAsync<RpcException>(() => client.CalculateAsync(request).ResponseAsync);

        Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
    }
}