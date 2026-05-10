using System.Globalization;
using Loyalty.Domain.Exceptions;

namespace Loyalty.Domain.ValueObjects;

public class PurchaseValue
{
    public decimal Value { get; }

    private PurchaseValue(decimal value)
    {
        Value = value;
    }

    public static PurchaseValue From(decimal value)
    {
        if (value < 0)
            throw new DomainValidationException("Purchase value cannot be negative.");

        return new PurchaseValue(value);
    }

    public override string ToString() => Value.ToString(CultureInfo.CurrentCulture);
}