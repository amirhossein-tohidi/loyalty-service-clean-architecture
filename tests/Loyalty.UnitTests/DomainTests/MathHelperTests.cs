using Loyalty.Domain.Helpers;
using Xunit;

namespace Loyalty.UnitTests.DomainTests;

public class MathHelperTests
{
    [Theory]
    [InlineData(100, 10, 10)]
    [InlineData(200, 50, 100)]
    [InlineData(150, 0, 0)]
    [InlineData(500, 5, 25)]
    public void Percent_ShouldReturnCorrectValue(decimal value, decimal percent, decimal expected)
    {
        var result = value.Percent(percent);

        Assert.Equal(expected, result);
    }
}