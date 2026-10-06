using Domain.Tenancy;
using Microsoft.Extensions.Configuration;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture.Tests;

public class TenantFoundationTests
{
    [Fact]
    public void Domain_ShouldDefineTenantFoundationTypes()
    {
        var domainAssembly = typeof(Domain.Entities.BaseEntity).Assembly;

        var tenantType = domainAssembly.GetType("Domain.Entities.Tenant");
        var tenantScopedType = domainAssembly.GetType("Domain.Tenancy.ITenantScoped");
        var tenantContextType = domainAssembly.GetType("Domain.Tenancy.TenantContext");
        var tenantContextAccessorType = domainAssembly.GetType("Domain.Tenancy.ITenantContextAccessor");

        tenantType.Should().NotBeNull();
        tenantType!.BaseType.Should().Be(typeof(Domain.Entities.BaseEntity));
        tenantType.GetProperty("Slug")!.PropertyType.Should().Be(typeof(string));
        tenantType.GetProperty("Name")!.PropertyType.Should().Be(typeof(string));

        tenantScopedType.Should().NotBeNull();
        tenantScopedType!.GetProperty("TenantId")!.PropertyType.Should().Be(typeof(Guid));

        tenantContextType.Should().NotBeNull();
        tenantContextType!.GetProperty("TenantId")!.PropertyType.Should().Be(typeof(Guid));
        tenantContextType.GetProperty("TenantSlug")!.PropertyType.Should().Be(typeof(string));
        tenantContextType.GetProperty("UserId")!.PropertyType.Should().Be(typeof(Guid?));

        tenantContextAccessorType.Should().NotBeNull();
        tenantContextAccessorType!.GetProperty("Current")!.PropertyType.Should().Be(tenantContextType);
    }

    [Fact]
    public void Infrastructure_ShouldRegisterScopedTenantContextAccessor()
    {
        var services = new ServiceCollection();
        services.AddInfrastructure(new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["DB_CONNECTION_STRING"] = "Host=unused" })
            .Build());

        var registration = services.Single(descriptor => descriptor.ServiceType == typeof(ITenantContextAccessor));
        registration.Lifetime.Should().Be(ServiceLifetime.Scoped);
        registration.ImplementationType.Should().Be(typeof(Infrastructure.Tenancy.TenantContextAccessor));

        var accessor = new Infrastructure.Tenancy.TenantContextAccessor();
        accessor.Current.Should().Be(TenantContext.Empty);
        var context = new TenantContext(Guid.CreateVersion7(), "school", Guid.CreateVersion7());
        accessor.Set(context);
        accessor.Current.Should().Be(context);
        accessor.Clear();
        accessor.Current.Should().Be(TenantContext.Empty);
        ((Action)(() => accessor.Set(null!))).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void TenantEntity_ShouldEnforceInvariantsAndIdempotentSoftDelete()
    {
        var tenant = Domain.Entities.Tenant.Create("  learning-school ", " Learning school ");
        tenant.Slug.Should().Be("learning-school");
        tenant.Name.Should().Be("Learning school");
        tenant.Active.Should().BeTrue();

        ((Action)(() => Domain.Entities.Tenant.Create(" ", "Name"))).Should().Throw<Domain.Common.DomainException>();
        ((Action)(() => Domain.Entities.Tenant.Create("slug", ""))).Should().Throw<Domain.Common.DomainException>();
        ((Action)(() => Domain.Entities.Tenant.Remove(null!))).Should().Throw<ArgumentNullException>();

        Domain.Entities.Tenant.Remove(tenant);
        var deletedAt = tenant.DeletedAt;
        deletedAt.Should().NotBeNull();
        tenant.Active.Should().BeFalse();

        Domain.Entities.Tenant.Remove(tenant);
        tenant.DeletedAt.Should().Be(deletedAt, "removing twice keeps the original deletion time");
    }
}
