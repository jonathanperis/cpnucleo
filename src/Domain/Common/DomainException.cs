namespace Domain.Common;

/// <summary>
/// A business-rule violation whose message is safe to show to API clients.
/// Both transports translate it into a validation failure (HTTP 400 / gRPC InvalidArgument).
/// </summary>
public sealed class DomainException(string message, string? field = null) : ArgumentException(message)
{
    /// <summary>The request field that violated the rule, when the rule is tied to one.</summary>
    public string? Field { get; } = field;
}
