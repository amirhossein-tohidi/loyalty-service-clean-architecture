namespace Loyalty.Domain.Helpers;

public static class MathHelper
{
    public static decimal Percent(this decimal value, decimal percent)
    {
        return value * percent / 100m;
    }
}