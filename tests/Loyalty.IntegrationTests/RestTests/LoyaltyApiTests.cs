using System.Net;
using System.Text.Json;
using Loyalty.Domain.Enums;
using Loyalty.IntegrationTests.Fixtures;
using Loyalty.IntegrationTests.Helpers;
using Xunit;

namespace Loyalty.IntegrationTests.RestTests;

public class LoyaltyApiTests(ApiWebApplicationFactory factory)
    : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task Calculate_ShouldReturnFinalScore()
    {
        var body = new
        {
            purchaseAmount = 15_000_000,
            customerType = (int)CustomerType.Gold,
            lastMonthPurchases = new[]
            {
                2_000_000,
                3_000_000,
                1_500_000,
                4_000_000,
                1_000_000
            }
        };
        var client = factory.CreateClient();

        var response = await client.PostAsync(
            "/api/v1/loyalty/calculate",
            JsonHelper.ToJson(body));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonSerializer.Deserialize<Dictionary<string, decimal>>(
            await response.Content.ReadAsStringAsync());

        Assert.NotNull(json);
        Assert.True(json.ContainsKey("finalScore"));
        Assert.Equal(287500, json["finalScore"]);
    }

    [Fact]
    public async Task Calculate_ShouldReturnFinalScore_AccordingToBusinessRules()
    {
        // Gold => base = 15,000,000 / 60 = 250,000
        // 5 purchases => +10%
        // amount >= 10M => +5%
        // extra = 5,000,000 => no additional 2.5%
        // total bonus = 15%
        // final = 250,000 * 1.15 = 287,500
        var body = new
        {
            purchaseAmount = 15_000_000m,
            customerType = (int)CustomerType.Gold,
            lastMonthPurchases = new[]
            {
                2_000_000m,
                3_000_000m,
                1_500_000m,
                4_000_000m,
                1_000_000m
            }
        };
        var client = factory.CreateClient();
        var response = await client.PostAsync(
            "/api/v1/loyalty/calculate",
            JsonHelper.ToJson(body));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonSerializer.Deserialize<Dictionary<string, decimal>>(
            await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(json);
        Assert.True(json!.ContainsKey("finalScore"));

        Assert.Equal(287_500m, json["finalScore"]);
    }

    [Fact]
    public async Task Calculate_WhenPurchaseAmountIsZero_ShouldReturnBadRequest()
    {
        var body = new
        {
            purchaseAmount = 0m,
            customerType = (int)CustomerType.Bronze,
            lastMonthPurchases = Array.Empty<decimal>()
        };
        var client = factory.CreateClient();
        var response = await client.PostAsync(
            "/api/v1/loyalty/calculate",
            JsonHelper.ToJson(body));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Validation failed.", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CalculateLoyaltyScore_WhenInvalidInput_ShouldReturnBadRequest()
    {
        var body = new
        {
            PurchaseAmount = 0,
            CustomerType = (int)CustomerType.Bronze,
            LastMonthPurchases = new List<decimal>()
        };
        var client = factory.CreateClient();
        var response = await client.PostAsync("/api/v1/loyalty/calculate", JsonHelper.ToJson(body));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Calculate_BronzeWithoutBonus_ShouldReturnExpectedScore()
    {
        var body = new
        {
            purchaseAmount = 1_000_000m,
            customerType = (int)CustomerType.Bronze,
            lastMonthPurchases = new[] { 100m, 200m }
        };

        var client = factory.CreateClient();
        var response = await client.PostAsync("/api/v1/loyalty/calculate", JsonHelper.ToJson(body));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = JsonSerializer.Deserialize<Dictionary<string, decimal>>(
            await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.NotNull(json);
        Assert.Equal(10_000m, json!["finalScore"]);
    }

    [Fact]
    public async Task Calculate_WhenPurchaseAmountIsNegative_ShouldReturnBadRequest()
    {
        var body = new
        {
            purchaseAmount = -1m,
            customerType = (int)CustomerType.Bronze,
            lastMonthPurchases = Array.Empty<decimal>()
        };

        var client = factory.CreateClient();
        var response = await client.PostAsync("/api/v1/loyalty/calculate", JsonHelper.ToJson(body));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}