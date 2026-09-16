using System.Reflection;

namespace Architecture.Tests;

public class ArchitectureTests
{
    private static readonly Assembly[] Assemblies =
    [
        typeof(Domain.Entities.BaseEntity).Assembly,
        typeof(Application.Features.Projects.CreateProject.CreateProjectHandler).Assembly,
        typeof(Infrastructure.DependencyInjection).Assembly,
        typeof(WebApi.Endpoints.Project.CreateProject.Endpoint).Assembly,
        typeof(IdentityApi.Endpoints.Login.Endpoint).Assembly,
        typeof(GrpcServer.Handlers.Project.CreateProjectHandler).Assembly,
        typeof(GrpcServer.Contracts.Commands.Project.CreateProjectCommand).Assembly
    ];

    public static TheoryData<string, string[]> DependencyBoundaries => new()
    {
        { "Domain", ["Application", "Infrastructure", "WebApi", "IdentityApi", "GrpcServer", "GrpcServer.Contracts", "Microsoft.EntityFrameworkCore", "Dapper", "Npgsql"] },
        { "Application", ["Infrastructure", "WebApi", "IdentityApi", "GrpcServer", "GrpcServer.Contracts"] },
        { "Infrastructure", ["WebApi", "IdentityApi", "GrpcServer", "GrpcServer.Contracts"] },
        { "WebApi", ["IdentityApi", "GrpcServer", "GrpcServer.Contracts"] },
        { "IdentityApi", ["WebApi", "GrpcServer", "GrpcServer.Contracts"] },
        { "GrpcServer", ["WebApi", "IdentityApi"] },
        { "GrpcServer.Contracts", ["Infrastructure", "WebApi", "IdentityApi"] }
    };

    [Theory]
    [MemberData(nameof(DependencyBoundaries))]
    public void Layers_ShouldRejectEachForbiddenDependency(string name, string[] forbidden)
    {
        var assembly = Assemblies.Single(a => a.GetName().Name == name);
        foreach (var dependency in forbidden)
        {
            var result = Types.InAssembly(assembly).ShouldNot().HaveDependencyOn(dependency).GetResult();
            result.IsSuccessful.Should().BeTrue($"{name} must not reference {dependency}");
        }
    }

    [Fact]
    public void DomainEntities_ShouldBeSealedAndInheritBaseEntity()
    {
        var entities = typeof(Domain.Entities.BaseEntity).Assembly.GetTypes()
            .Where(t => t.Namespace == "Domain.Entities" && !t.IsAbstract).ToArray();
        entities.Should().NotBeEmpty();
        entities.Should().OnlyContain(t => t.IsSealed && t.IsSubclassOf(typeof(Domain.Entities.BaseEntity)));
    }

    [Fact]
    public void Repositories_ShouldImplementDomainPorts()
    {
        var repositories = typeof(Infrastructure.DependencyInjection).Assembly.GetTypes()
            .Where(t => t.Namespace == "Infrastructure.Repositories" && t.IsClass && !t.IsNested).ToArray();
        repositories.Should().NotBeEmpty();
        repositories.Should().OnlyContain(t => t.GetInterfaces().Any(i => i.Namespace == "Domain.Repositories"));
    }

    [Theory]
    [InlineData("WebApi")]
    [InlineData("IdentityApi")]
    public void HttpEndpoints_ShouldHaveConventionalNames(string name)
    {
        var endpoints = Assemblies.Single(a => a.GetName().Name == name).GetTypes()
            .Where(t => t.IsSubclassOf(typeof(FastEndpoints.BaseEndpoint)) && !t.IsAbstract).ToArray();
        endpoints.Should().NotBeEmpty();
        endpoints.Should().OnlyContain(t => t.Name == "Endpoint" && t.Namespace!.StartsWith(name + ".Endpoints."));
    }

    [Fact]
    public void GrpcHandlers_ShouldUseApplicationOrDomainContracts()
    {
        var handlers = typeof(GrpcServer.Handlers.Project.CreateProjectHandler).Assembly.GetTypes()
            .Where(t => t.Namespace?.StartsWith("GrpcServer.Handlers.") == true && !t.IsNested && t.IsClass).ToArray();
        handlers.Should().HaveCount(55);
        handlers.Should().OnlyContain(t => t.IsSealed && t.Name.EndsWith("Handler"));
    }

    [Fact]
    public void Dtos_ShouldUseExplicitDtoNames()
    {
        foreach (var name in new[] { "WebApi", "GrpcServer.Contracts" })
        {
            var dtos = Assemblies.Single(a => a.GetName().Name == name).GetTypes()
                .Where(t => t.Namespace == name + ".Common.Dtos" && t.IsClass).ToArray();
            dtos.Should().NotBeEmpty();
            dtos.Should().OnlyContain(t => t.Name.EndsWith("Dto"));
        }
    }
}
