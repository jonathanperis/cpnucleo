using System.Security.Cryptography;
using System.Text;

namespace Domain.Common.Security;

/// <summary>
/// A short fingerprint of the credentials a token was issued for. It changes whenever the
/// password hash or login changes, so tokens issued before such a change stop validating.
/// </summary>
public static class SecurityStamp
{
    public static string Compute(string? passwordHash, string? login)
    {
        var material = Encoding.UTF8.GetBytes($"{passwordHash}\n{login?.Trim().ToLowerInvariant()}");
        return Convert.ToBase64String(SHA256.HashData(material).AsSpan(0, 16))
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    public static string Compute(User user) => Compute(user.Password, user.Login);
}
