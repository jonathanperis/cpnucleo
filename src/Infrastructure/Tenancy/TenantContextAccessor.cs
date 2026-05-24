namespace Infrastructure.Tenancy;

public sealed class TenantContextAccessor : ITenantContextAccessor
{
    public TenantContext Current { get; private set; } = TenantContext.Empty;

    public void Set(TenantContext context)
    {
        Current = context;
    }

    public void Clear()
    {
        Current = TenantContext.Empty;
    }
}
