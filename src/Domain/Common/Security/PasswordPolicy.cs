using Domain.Common;

namespace Domain.Common.Security;

/// <summary>
/// Password rules shared by REST validators and gRPC handlers.
/// </summary>
public static class PasswordPolicy
{
    public const int MinimumLength = 8;
    public const int MaximumLength = 128;
    public const string AllowedSymbols = "@$!%*?&";
    public const string Description = "Password must be 8–128 characters and contain at least one uppercase letter, one lowercase letter, one digit, and one special character (@$!%*?&).";

    public static bool IsSatisfiedBy(string? password) =>
        password is { Length: >= MinimumLength and <= MaximumLength }
        && password.All(IsAllowed)
        && password.Any(char.IsAsciiLetterLower)
        && password.Any(char.IsAsciiLetterUpper)
        && password.Any(char.IsAsciiDigit)
        && password.Any(AllowedSymbols.Contains);

    public static void Validate(string? password)
    {
        if (!IsSatisfiedBy(password)) throw new DomainException(Description, "Password");
    }

    private static bool IsAllowed(char character) => char.IsAsciiLetterOrDigit(character) || AllowedSymbols.Contains(character);
}
