namespace Infrastructure.Security;

/// <summary>
/// Administrators are configured by login in <c>CPNUCLEO_ADMIN_LOGINS</c> (comma-separated). When
/// nothing is configured, the seeded <see cref="DefaultLogin"/> account is the administrator, so a
/// fresh deployment always has one. An explicit list replaces the default entirely.
/// </summary>
public static class AdminLogins
{
    public const string ConfigurationKey = "CPNUCLEO_ADMIN_LOGINS";

    /// <summary>The account created by the lab seeder and the demo data importer.</summary>
    public const string DefaultLogin = "demo@cpnucleo.local";

    public static IReadOnlyList<string> Configured(IConfiguration configuration)
    {
        var configured = (configuration[ConfigurationKey] ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return configured.Length > 0 ? configured : [DefaultLogin];
    }

    public static bool Contains(IConfiguration configuration, string? login) =>
        !string.IsNullOrWhiteSpace(login) &&
        Configured(configuration).Contains(login.Trim(), StringComparer.OrdinalIgnoreCase);
}
