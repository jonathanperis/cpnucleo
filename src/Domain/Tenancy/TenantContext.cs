namespace Domain.Tenancy;

public sealed record TenantContext(Guid TenantId, string TenantSlug, Guid? UserId)
{
    public static TenantContext Empty { get; } = new(Guid.Empty, string.Empty, null);

    public bool IsResolved => TenantId != Guid.Empty;
}
