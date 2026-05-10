namespace Loyalty.Api.Common;

public sealed class ErrorResponse
{
    public string Message { get; init; } = default!;
    public IDictionary<string, string[]>? Errors { get; init; }
    public string? TraceId { get; init; }
}