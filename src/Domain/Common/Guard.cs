namespace Domain.Common;

/// <summary>
/// Shared invariant checks so every transport and persistence style enforces identical rules.
/// </summary>
public static class Guard
{
    public const int NameMaxLength = 200;
    public const int DescriptionMaxLength = 2000;
    public const int LoginMaxLength = 256;

    public static string Required(string? value, string field, int maxLength = NameMaxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new DomainException($"{field} is required.", field);
        var trimmed = value.Trim();
        if (trimmed.Length > maxLength) throw new DomainException($"{field} must be at most {maxLength} characters.", field);
        return trimmed;
    }

    public static string? Optional(string? value, string field, int maxLength = DescriptionMaxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var trimmed = value.Trim();
        if (trimmed.Length > maxLength) throw new DomainException($"{field} must be at most {maxLength} characters.", field);
        return trimmed;
    }

    public static Guid Reference(Guid value, string field)
    {
        if (value == Guid.Empty) throw new DomainException($"{field} is required.", field);
        return value;
    }

    public static int Positive(int value, string field)
    {
        if (value <= 0) throw new DomainException($"{field} must be greater than 0.", field);
        return value;
    }

    /// <summary>
    /// Normalizes a timestamp to UTC. Unspecified values (for example "2064-06-09" or a value
    /// without an offset) are treated as UTC; local values are converted.
    /// </summary>
    public static DateTime Utc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };

    public static DateTime RequiredUtc(DateTime value, string field)
    {
        if (value == default) throw new DomainException($"{field} is required.", field);
        return Utc(value);
    }
}
