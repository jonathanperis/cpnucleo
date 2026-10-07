using Microsoft.AspNetCore.Http;

namespace Infrastructure.Http;

/// <summary>Request values made safe for log entries (no forged lines through CR/LF).</summary>
public static class LogValues
{
    public static string Clean(string? value) =>
        (value ?? string.Empty).Replace("\r", string.Empty, StringComparison.Ordinal).Replace("\n", string.Empty, StringComparison.Ordinal);

    public static string Method(HttpRequest request) => Clean(request.Method);

    public static string Path(HttpRequest request) => Clean(request.Path.Value);
}
