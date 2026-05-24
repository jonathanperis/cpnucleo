namespace Domain.Tenancy;

public interface ITenantContextAccessor
{
    TenantContext Current { get; }
}
