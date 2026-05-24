namespace Infrastructure.Tenancy;

public sealed class TenantContextAccessor : ITenantContextAccessor
{
    private static readonly AsyncLocal<TenantContext?> CurrentContext = new();

    public TenantContext Current => CurrentContext.Value ?? TenantContext.Empty;

    public void Set(TenantContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        CurrentContext.Value = context;
    }

    public void Clear()
    {
        CurrentContext.Value = null;
    }
}
