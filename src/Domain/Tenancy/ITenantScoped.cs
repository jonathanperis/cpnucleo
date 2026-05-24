namespace Domain.Tenancy;

public interface ITenantScoped
{
    Guid TenantId { get; }
}
