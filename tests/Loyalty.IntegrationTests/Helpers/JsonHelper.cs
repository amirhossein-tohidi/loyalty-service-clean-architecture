using System.Text.Json;

namespace Loyalty.IntegrationTests.Helpers;

public static class JsonHelper
{
    public static StringContent ToJson(object obj)
    {
        var json = JsonSerializer.Serialize(obj);
        return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    }
}