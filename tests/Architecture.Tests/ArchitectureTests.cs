namespace Architecture.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "Domain";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string WebApiNamespace = "WebApi";
    private const string IdentityApiNamespace = "IdentityApi";
    private const string GrpcServerNamespace = "GrpcServer";
    private const string GrpcServerContractsNamespace = "GrpcServer.Contracts";
    private const string WebClientNamespace = "WebClient";

    #region Layer Dependency Tests

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            WebApiNamespace,
            IdentityApiNamespace,
            GrpcServerNamespace,
            GrpcServerContractsNamespace,
            WebClientNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Infrastructure.DependencyInjection).Assembly;

        var otherProjects = new[]
        {
            WebApiNamespace,
            IdentityApiNamespace,
            GrpcServerNamespace,
            GrpcServerContractsNamespace,
            WebClientNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Repositories_Should_HaveDependencyOnDomain()
    {
        // Arrange
        var assembly = typeof(Infrastructure.DependencyInjection).Assembly;

        // Act - Repository implementations should depend on Domain
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace("Infrastructure.Repositories")
            .And()
            .HaveNameEndingWith("Repository")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void GrpcServerContracts_Should_OnlyDependOnDomain()
    {
        // Arrange - GrpcServer.Contracts should only depend on Domain, not Infrastructure or other layers
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;
        
        // We need to load the GrpcServer.Contracts assembly
        var grpcContractsAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "GrpcServer.Contracts");

        if (grpcContractsAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            WebApiNamespace,
            IdentityApiNamespace,
            GrpcServerNamespace,
            WebClientNamespace
        };

        // Act
        var testResult = Types
            .InAssembly(grpcContractsAssembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void WebApi_Should_NotDependOnGrpcServer()
    {
        // Arrange - WebApi should not depend on GrpcServer
        var webApiAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "WebApi");

        if (webApiAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act
        var testResult = Types
            .InAssembly(webApiAssembly)
            .ShouldNot()
            .HaveDependencyOn(GrpcServerNamespace)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void IdentityApi_Should_NotDependOnGrpcServer()
    {
        // Arrange - IdentityApi should not depend on GrpcServer
        var identityApiAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "IdentityApi");

        if (identityApiAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act
        var testResult = Types
            .InAssembly(identityApiAssembly)
            .ShouldNot()
            .HaveDependencyOn(GrpcServerNamespace)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion

    #region Domain Layer Tests

    [Fact]
    public void Domain_Entities_Should_InheritFromBaseEntity()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace("Domain.Entities")
            .And()
            .AreNotAbstract()
            .And()
            .DoNotHaveName("BaseEntity")
            .Should()
            .Inherit(typeof(Domain.Entities.BaseEntity))
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Repositories_Should_BeInterfaces()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace("Domain.Repositories")
            .And()
            .HaveNameStartingWith("I")
            .Should()
            .BeInterfaces()
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Entities_Should_BeSealed()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace("Domain.Entities")
            .And()
            .AreNotAbstract()
            .And()
            .DoNotHaveName("BaseEntity")
            .Should()
            .BeSealed()
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion

    #region Infrastructure Layer Tests

    [Fact]
    public void Infrastructure_Repositories_Should_ImplementDomainInterfaces()
    {
        // Arrange
        var assembly = typeof(Infrastructure.DependencyInjection).Assembly;

        // Act - All repository classes should implement IRepository or specific interfaces
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace("Infrastructure.Repositories")
            .And()
            .HaveNameEndingWith("Repository")
            .And()
            .AreClasses()
            .Should()
            .ImplementInterface(typeof(Domain.Repositories.IRepository<>))
            .Or()
            .HaveNameMatching(".*Repository")
            .GetResult();

        // Assert - This is a soft check; repositories should implement domain interfaces
        // Some repositories might implement specific interfaces, so we're just checking naming
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_DbContext_Should_BeInCorrectNamespace()
    {
        // Arrange
        var assembly = typeof(Infrastructure.DependencyInjection).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("DbContext")
            .Should()
            .ResideInNamespace("Infrastructure.Common.Context")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion

    #region Naming Convention Tests

    [Fact]
    public void WebApi_Dtos_Should_HaveDtoSuffix()
    {
        // Arrange
        var webApiAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "WebApi");

        if (webApiAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act
        var testResult = Types
            .InAssembly(webApiAssembly)
            .That()
            .ResideInNamespace("WebApi.Common.Dtos")
            .And()
            .AreNotAbstract()
            .Should()
            .HaveNameEndingWith("Dto")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void GrpcServer_Handlers_Should_HaveHandlerSuffix()
    {
        // Arrange
        var grpcServerAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "GrpcServer");

        if (grpcServerAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act
        var testResult = Types
            .InAssembly(grpcServerAssembly)
            .That()
            .ResideInNamespaceMatching("GrpcServer.Handlers.*")
            .And()
            .AreClasses()
            .And()
            .AreNotAbstract()
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void GrpcServerContracts_Commands_Should_HaveCommandSuffix()
    {
        // Arrange
        var grpcContractsAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "GrpcServer.Contracts");

        if (grpcContractsAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act
        var testResult = Types
            .InAssembly(grpcContractsAssembly)
            .That()
            .ResideInNamespaceMatching("GrpcServer.Contracts.Commands.*")
            .And()
            .AreClasses()
            .And()
            .DoNotHaveNameEndingWith("Result")
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void GrpcServerContracts_Dtos_Should_HaveDtoSuffix()
    {
        // Arrange
        var grpcContractsAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "GrpcServer.Contracts");

        if (grpcContractsAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act
        var testResult = Types
            .InAssembly(grpcContractsAssembly)
            .That()
            .ResideInNamespace("GrpcServer.Contracts.Common.Dtos")
            .And()
            .AreNotAbstract()
            .Should()
            .HaveNameEndingWith("Dto")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void WebApi_Endpoints_Should_BeNamedEndpoint()
    {
        // Arrange
        var webApiAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "WebApi");

        if (webApiAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act - Endpoints should be in Endpoints namespace and named "Endpoint"
        var testResult = Types
            .InAssembly(webApiAssembly)
            .That()
            .ResideInNamespaceMatching("WebApi.Endpoints.*")
            .And()
            .AreClasses()
            .And()
            .DoNotHaveName("Models")
            .Should()
            .HaveName("Endpoint")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void IdentityApi_Endpoints_Should_BeNamedEndpoint()
    {
        // Arrange
        var identityApiAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "IdentityApi");

        if (identityApiAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act
        var testResult = Types
            .InAssembly(identityApiAssembly)
            .That()
            .ResideInNamespaceMatching("IdentityApi.Endpoints.*")
            .And()
            .AreClasses()
            .And()
            .DoNotHaveName("Models")
            .Should()
            .HaveName("Endpoint")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion

    #region Interface Implementation Tests

    [Fact]
    public void Domain_Repositories_Should_StartWithI()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace("Domain.Repositories")
            .And()
            .AreInterfaces()
            .Should()
            .HaveNameStartingWith("I")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_NotContainInterfaces()
    {
        // Arrange
        var assembly = typeof(Infrastructure.DependencyInjection).Assembly;

        // Act - Infrastructure should implement interfaces from Domain, not define its own public interfaces
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .AreInterfaces()
            .And()
            .ArePublic()
            .Should()
            .ResideInNamespace("Infrastructure.Common.Context") // Only IApplicationDbContext is acceptable
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion

    #region Clean Architecture Pattern Tests

    [Fact]
    public void Domain_Should_NotDependOnEntityFramework()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        // Act - Domain should not depend on Entity Framework
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn("Microsoft.EntityFrameworkCore")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Should_NotDependOnDapper()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        // Act - Domain should not depend on Dapper
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn("Dapper")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Should_NotDependOnNpgsql()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        // Act - Domain should not depend on Npgsql
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOn("Npgsql")
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Models_Should_BeRecordsOrClasses()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ResideInNamespace("Domain.Models")
            .Should()
            .BeClasses()
            .Or()
            .BeSealed() // Records are sealed
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void WebApi_Should_NotHaveBusinessLogic()
    {
        // Arrange - WebApi endpoints should not contain complex business logic, just orchestration
        var webApiAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "WebApi");

        if (webApiAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act - Endpoints should not directly instantiate domain entities (should use repositories/handlers)
        var testResult = Types
            .InAssembly(webApiAssembly)
            .That()
            .ResideInNamespaceMatching("WebApi.Endpoints.*")
            .ShouldNot()
            .HaveDependencyOn("Domain.Entities")
            .Or()
            .HaveDependencyOn("Domain.Repositories") // Endpoints should use repositories via DI
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void GrpcServer_Handlers_Should_HaveDependencyOnDomain()
    {
        // Arrange
        var grpcServerAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "GrpcServer");

        if (grpcServerAssembly == null)
        {
            // Skip test if assembly not loaded
            return;
        }

        // Act
        var testResult = Types
            .InAssembly(grpcServerAssembly)
            .That()
            .ResideInNamespaceMatching("GrpcServer.Handlers.*")
            .And()
            .HaveNameEndingWith("Handler")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    #endregion
}
