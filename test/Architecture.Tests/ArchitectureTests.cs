namespace Architecture.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "Domain";
    private const string ApplicationNamespace = "Application";
    private const string InfrastructureNamespace = "Infrastructure";
    private const string ServicesNamespace = "WebApi";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Domain.Entities.BaseEntity).Assembly;

        var otherProjects = new[]
        {
            ApplicationNamespace,
            InfrastructureNamespace,
            ServicesNamespace
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
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        // Arrange
        var assembly = typeof(Application.DependencyInjection).Assembly;

        var otherProjects = new[]
        {
            InfrastructureNamespace,
            ServicesNamespace
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
            ServicesNamespace
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

    // [Fact]
    // public void Handlers_Should_Have_DependencyOnDomain()
    // {
    //     // Arrange
    //     var assembly = typeof(Application.DependencyInjection).Assembly;

    //     // Act
    //     var testResult = Types
    //         .InAssembly(assembly)
    //         .That()
    //         .HaveNameEndingWith("Handler")
    //         .Should()
    //         .HaveDependencyOn(DomainNamespace)
    //         .GetResult();

    //     // Assert
    //     testResult.IsSuccessful.Should().BeTrue();
    // }

    // [Fact]
    // public void Services_Should_HaveDependencyOnMediator()
    // {
    //     // Arrange
    //     var assembly = typeof(WebUI.Modules.TrackingTransportationModule).Assembly;

    //     // Act
    //     var testResult = Types
    //         .InAssembly(assembly)
    //         .That()
    //         .HaveNameEndingWith("Module")
    //         .Should()
    //         .HaveDependencyOn("Mediator")
    //         .GetResult();

    //     // Assert
    //     testResult.IsSuccessful.Should().BeTrue();
    // }
}
