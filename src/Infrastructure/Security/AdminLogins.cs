namespace Infrastructure.Security;

/// <summary>Administrators are configured by login in <c>CPNUCLEO_ADMIN_LOGINS</c> (comma-separated).</summary>
public static class AdminLogins
{
    public const string ConfigurationKey = "CPNUCLEO_ADMIN_LOGINS";

    public static bool Contains(IConfiguration configuration, string? login) =>
        !string.IsNullOrWhiteSpace(login) &&
        (configuration[ConfigurationKey] ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Contains(login.Trim(), StringComparer.OrdinalIgnoreCase);
}
