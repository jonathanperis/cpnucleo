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
        var dependencyInjection = File.ReadAllText(GetRepositoryPath("src/Infrastructure/DependencyInjection.cs"));
        var accessor = File.ReadAllText(GetRepositoryPath("src/Infrastructure/Tenancy/TenantContextAccessor.cs"));

        dependencyInjection.Should().Contain("AddScoped<ITenantContextAccessor, TenantContextAccessor>()");
        accessor.Should().Contain("class TenantContextAccessor");
        accessor.Should().Contain("ITenantContextAccessor");
        accessor.Should().Contain("TenantContext.Empty");
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
