using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Loyalty.Application.Extensions;

public static class EnumExtensions
{
    public static int ToInt(this Enum enumName)
    {
        return Convert.ToInt32(enumName);
    }

    public static IEnumerable<T> GetEnumValues<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        return Enum.GetValues(input.GetType()).Cast<T>();
    }

    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetName() ?? enumValue.ToString();
    }
}