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
        var slugProperty = tenantType.GetProperty("Slug");
        slugProperty.Should().NotBeNull();
        slugProperty!.PropertyType.Should().Be(typeof(string));

        var nameProperty = tenantType.GetProperty("Name");
        nameProperty.Should().NotBeNull();
        nameProperty!.PropertyType.Should().Be(typeof(string));

        tenantScopedType.Should().NotBeNull();
        var tenantIdProperty = tenantScopedType!.GetProperty("TenantId");
        tenantIdProperty.Should().NotBeNull();
        tenantIdProperty!.PropertyType.Should().Be(typeof(Guid));

        tenantContextType.Should().NotBeNull();
        var contextTenantIdProperty = tenantContextType!.GetProperty("TenantId");
        contextTenantIdProperty.Should().NotBeNull();
        contextTenantIdProperty!.PropertyType.Should().Be(typeof(Guid));

        var tenantSlugProperty = tenantContextType.GetProperty("TenantSlug");
        tenantSlugProperty.Should().NotBeNull();
        tenantSlugProperty!.PropertyType.Should().Be(typeof(string));

        var userIdProperty = tenantContextType.GetProperty("UserId");
        userIdProperty.Should().NotBeNull();
        userIdProperty!.PropertyType.Should().Be(typeof(Guid?));

        tenantContextAccessorType.Should().NotBeNull();
        var currentProperty = tenantContextAccessorType!.GetProperty("Current");
        currentProperty.Should().NotBeNull();
        currentProperty!.PropertyType.Should().Be(tenantContextType);
    }

    [Fact]
    public void Infrastructure_ShouldRegisterScopedTenantContextAccessor()
    {
        var dependencyInjection = File.ReadAllText(GetRepositoryPath("src/Infrastructure/DependencyInjection.cs"));
        var accessor = File.ReadAllText(GetRepositoryPath("src/Infrastructure/Tenancy/TenantContextAccessor.cs"));

        dependencyInjection.Should().Contain("AddScoped<ITenantContextAccessor, TenantContextAccessor>()");
        accessor.Should().Contain("class TenantContextAccessor");
        accessor.Should().Contain("ITenantContextAccessor");
        accessor.Should().Contain("TenantContext.Empty");
        accessor.Should().Contain("AsyncLocal<TenantContext?>");
        accessor.Should().Contain("ArgumentNullException.ThrowIfNull(context)");
    }

    [Fact]
    public void TenantEntity_ShouldEnforceInvariantsAndIdempotentSoftDelete()
    {
        var tenantSource = File.ReadAllText(GetRepositoryPath("src/Domain/Entities/Tenant.cs"));

        tenantSource.Should().Contain("string.IsNullOrWhiteSpace(slug)");
        tenantSource.Should().Contain("string.IsNullOrWhiteSpace(name)");
        tenantSource.Should().Contain("slug.Trim()");
        tenantSource.Should().Contain("name.Trim()");
        tenantSource.Should().Contain("ArgumentNullException.ThrowIfNull(obj)");
        tenantSource.Should().Contain("if (!obj.Active)");
        tenantSource.Should().Contain("obj.DeletedAt ??= DateTime.UtcNow");
    }

    private static string GetRepositoryPath(string relativePath)
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            var candidate = Path.GetFullPath(Path.Combine(current.FullName, relativePath));
            if (File.Exists(Path.Combine(current.FullName, "cpnucleo.slnx")) ||
                Directory.Exists(Path.Combine(current.FullName, ".git")))
            {
                return candidate;
            }

            current = current.Parent;
        }

        return Path.GetFullPath(relativePath);
    }
}
