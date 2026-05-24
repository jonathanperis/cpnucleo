namespace Architecture.Tests;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;

public class FastEndpointsConfigurationTests
{
    private static readonly string[] HttpVerbRouteMethods = ["Get", "Post", "Put", "Delete", "Patch"];
    private static readonly string[] AuthenticationPackageNames = ["FastEndpoints.Security", "Microsoft.AspNetCore.Authentication.JwtBearer"];
    private static readonly string[] JwtValidationSnippets =
    [
        "AddAuthentication(JwtBearerDefaults.AuthenticationScheme)",
        "ValidateIssuerSigningKey = true",
        "ValidateLifetime = true",
        "ValidateIssuer = true",
        "ValidateAudience = true",
        "ValidIssuer = builder.Configuration[\"Jwt:Issuer\"]",
        "ValidAudience = builder.Configuration[\"Jwt:Audience\"]"
    ];

    [Fact]
    public void FastEndpointsPackageVersions_ShouldBeAligned()
    {
        var repositoryRoot = GetRepositoryPath(".");
        var fastEndpointsPackageVersions = Directory
            .EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                && !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .SelectMany(projectPath => XDocument
                .Load(projectPath)
                .Descendants("PackageReference")
                .Where(x => x.Attribute("Include")?.Value.StartsWith("FastEndpoints", StringComparison.Ordinal) == true)
                .Select(x => new
                {
                    ProjectPath = Path.GetRelativePath(repositoryRoot, projectPath),
                    Version = x.Attribute("Version")?.Value
                }))
            .ToArray();

        fastEndpointsPackageVersions.Should().NotBeEmpty();
        fastEndpointsPackageVersions
            .Select(x => x.Version)
            .Distinct()
            .Should()
            .ContainSingle("all FastEndpoints packages should use the same reviewed version: {0}", string.Join(", ", fastEndpointsPackageVersions.Select(x => $"{x.ProjectPath}: {x.Version}")))
            .Which.Should().Be("8.1.0");
    }

    [Fact]
    public void AuthenticationPackages_ShouldStayInIdentityAndApiHosts()
    {
        var repositoryRoot = GetRepositoryPath(".");
        var projectsWithAuthenticationPackages = Directory
            .EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                && !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .SelectMany(projectPath => XDocument
                .Load(projectPath)
                .Descendants("PackageReference")
                .Where(x => AuthenticationPackageNames.Contains(x.Attribute("Include")?.Value, StringComparer.Ordinal))
                .Select(x => Path.GetRelativePath(repositoryRoot, projectPath)))
            .Distinct()
            .ToArray();

        projectsWithAuthenticationPackages.Should().BeEquivalentTo(
        [
            "src/GrpcServer/GrpcServer.csproj",
            "src/IdentityApi/IdentityApi.csproj",
            "src/WebApi/WebApi.csproj"
        ]);
    }

    [Theory]
    [InlineData("src/WebApi/Program.cs")]
    [InlineData("src/GrpcServer/Program.cs")]
    public void ApiHosts_ShouldValidateIdentityApiBearerTokens(string programPath)
    {
        var program = File.ReadAllText(GetRepositoryPath(programPath));

        foreach (var snippet in JwtValidationSnippets)
        {
            program.Should().Contain(snippet);
        }

        program.Should().Contain("UseAuthentication()");
        program.Should().Contain("UseAuthorization()");
    }

    [Fact]
    public void GrpcServer_ShouldRequireAuthorizationForHandlers()
    {
        var program = File.ReadAllText(GetRepositoryPath("src/GrpcServer/Program.cs"));

        program.Should().Contain("MapHandlers(h =>");
        program.Should().Contain("FallbackPolicy = new AuthorizationPolicyBuilder()");
        program.Should().Contain("RequireAuthenticatedUser()");
    }

    [Theory]
    [InlineData("src/WebApi/Program.cs")]
    [InlineData("src/IdentityApi/Program.cs")]
    public void BrowserFacingApiHosts_ShouldAllowConfiguredWebClientCorsPreflight(string programPath)
    {
        var program = File.ReadAllText(GetRepositoryPath(programPath));
        var useCorsIndex = program.IndexOf("UseCors(\"CpnucleoWebClient\")", StringComparison.Ordinal);
        var useAuthenticationIndex = program.IndexOf("UseAuthentication()", StringComparison.Ordinal);

        program.Should().Contain("AddCors(options =>");
        program.Should().Contain("CpnucleoWebClient");
        program.Should().Contain("Cors:AllowedOrigins");
        program.Should().Contain("https://cpnucleo.jonathanperis.tech");
        program.Should().Contain("AllowAnyHeader()");
        program.Should().Contain("AllowAnyMethod()");
        useCorsIndex.Should().BeGreaterThanOrEqualTo(0);
        useAuthenticationIndex.Should().BeGreaterThan(useCorsIndex, "CORS middleware must run before authentication/authorization so browser preflights are answered");
    }

    [Theory]
    [InlineData("src/IdentityApi/appsettings.json")]
    [InlineData("src/IdentityApi/appsettings.Development.json")]
    [InlineData("src/WebApi/appsettings.json")]
    [InlineData("src/WebApi/appsettings.Development.json")]
    [InlineData("src/WebApi/appsettings.Testing.json")]
    [InlineData("src/GrpcServer/appsettings.json")]
    [InlineData("src/GrpcServer/appsettings.Development.json")]
    public void AuthConfiguration_ShouldUseCpnucleoJonathanPerisTechDomains(string appSettingsPath)
    {
        var appSettings = File.ReadAllText(GetRepositoryPath(appSettingsPath));

        appSettings.Should().Contain("\"Issuer\": \"https://identity-cpnucleo.jonathanperis.tech\"");
        appSettings.Should().Contain("\"Audience\": \"https://api-cpnucleo.jonathanperis.tech\"");
        appSettings.Should().NotContain("peris-studio.dev");
    }

    [Fact]
    public void WebApiEndpoints_ShouldRequireAuthorizationByDefault()
    {
        var endpointFiles = Directory.GetFiles(GetRepositoryPath("src/WebApi/Endpoints"), "Endpoint.cs", SearchOption.AllDirectories);
        var anonymousEndpoints = endpointFiles
            .Where(path => File.ReadAllText(path).Contains("AllowAnonymous();", StringComparison.Ordinal))
            .Select(path => Path.GetRelativePath(GetRepositoryPath("."), path))
            .ToArray();

        anonymousEndpoints.Should().BeEmpty("WebApi endpoints must require IdentityApi-issued bearer tokens");
    }

    [Theory]
    [InlineData("src/WebApi/ServiceExtensions/ConfigureOpenTelemetryOptions.cs", "webapi", true)]
    [InlineData("src/IdentityApi/ServiceExtensions/ConfigureOpenTelemetryOptions.cs", "identityapi", true)]
    [InlineData("src/GrpcServer/ServiceExtensions/ConfigureOpenTelemetryOptions.cs", "grpcserver", false)]
    public void DotNetHosts_ShouldExportRichOpenTelemetrySignals(string telemetryPath, string projectName, bool shouldIncludeEfCore)
    {
        var telemetry = File.ReadAllText(GetRepositoryPath(telemetryPath));
        var requiredSnippets = new[]
        {
            ".AddAspNetCoreInstrumentation(options =>",
            "options.RecordException = true",
            "EnrichWithHttpRequest",
            "http.request.host",
            "http.request.scheme",
            "http.request.protocol",
            "http.request.path",
            "http.request.query_string_length",
            "user_agent.original",
            "EnrichWithHttpResponse",
            "http.response.content_length",
            "http.response.content_type",
            "EnrichWithException",
            "exception.type",
            ".AddHttpClientInstrumentation(options =>",
            ".AddRuntimeInstrumentation()",
            ".AddProcessInstrumentation()",
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "System.Net.Http",
            "System.Net.NameResolution",
            "Npgsql",
            ".AddNpgsql()",
            ".AddNpgsqlInstrumentation",
            "Logging.AddOpenTelemetry",
            "IncludeFormattedMessage",
            "IncludeScopes",
            "ParseStateValues",
            "SetResourceBuilder",
            "serviceNamespace: \"cpnucleo\"",
            "deployment.environment",
            "host.name",
            "process.id",
            "process.runtime.name",
            "os.description",
            $"[\"cpnucleo.project\"] = \"{projectName}\"",
            "OTEL_EXPORTER_OTLP_ENDPOINT"
        };

        foreach (var snippet in requiredSnippets)
        {
            telemetry.Should().Contain(snippet);
        }

        if (shouldIncludeEfCore)
        {
            telemetry.Should().Contain(".AddEntityFrameworkCoreInstrumentation(options =>");
            telemetry.Should().Contain("EnrichWithIDbCommand");
            telemetry.Should().Contain("db.system");
            telemetry.Should().Contain("db.name");
            telemetry.Should().Contain("db.command.timeout");
        }
        else
        {
            telemetry.Should().NotContain(".AddEntityFrameworkCoreInstrumentation", "GrpcServer uses Dapper/Npgsql instead of EF Core");
        }
    }

    [Fact]
    public void WebClient_ShouldSendServerTelemetryToCollector()
    {
        var packageJson = File.ReadAllText(GetRepositoryPath("src/WebClient/package.json"));
        var dockerfile = File.ReadAllText(GetRepositoryPath("src/WebClient/Dockerfile"));
        var previewServer = File.ReadAllText(GetRepositoryPath("src/WebClient/scripts/preview.mjs"));
        var telemetry = File.ReadAllText(GetRepositoryPath("src/WebClient/scripts/otel.mjs"));
        var compose = File.ReadAllText(GetRepositoryPath("compose.yaml"));
        var prodCompose = File.ReadAllText(GetRepositoryPath("compose.prod.yaml"));

        foreach (var dependency in new[]
        {
            "@opentelemetry/sdk-node",
            "@opentelemetry/exporter-trace-otlp-http",
            "@opentelemetry/exporter-metrics-otlp-http",
            "@opentelemetry/exporter-logs-otlp-http",
            "@opentelemetry/auto-instrumentations-node"
        })
        {
            packageJson.Should().Contain(dependency);
        }

        previewServer.Should().Contain("./otel.mjs");
        telemetry.Should().Contain("service.name");
        telemetry.Should().Contain("WebClient-Cpnucleo");
        telemetry.Should().Contain("cpnucleo.project");
        telemetry.Should().Contain("webclient");
        telemetry.Should().Contain("OTEL_EXPORTER_OTLP_ENDPOINT");
        telemetry.Should().Contain("OTEL_EXPORTER_OTLP_HTTP_ENDPOINT");
        telemetry.Should().Contain("getNodeAutoInstrumentations");
        telemetry.Should().Contain("OTLPTraceExporter");
        telemetry.Should().Contain("OTLPMetricExporter");
        telemetry.Should().Contain("OTLPLogExporter");

        dockerfile.Should().Contain("FROM node:22-alpine AS runtime");
        dockerfile.Should().Contain("CMD [\"bun\", \"run\", \"preview\"]");

        compose.Should().Contain("OTEL_EXPORTER_OTLP_HTTP_ENDPOINT: http://otel-collector:4318");
        prodCompose.Should().Contain("OTEL_EXPORTER_OTLP_HTTP_ENDPOINT: http://otel-collector:4318");
    }

    [Fact]
    public void IdentityApi_ShouldRegisterFastEndpointsOnce()
    {
        var program = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Program.cs"));
        var registrationCount = CountInvocationExpressions(program, "AddFastEndpoints");

        registrationCount.Should().Be(1);
    }

    [Theory]
    [InlineData("src/WebApi/Program.cs")]
    [InlineData("src/IdentityApi/Program.cs")]
    public void ApiProjects_ShouldUseGlobalFastEndpointsApiRoutePrefix(string programPath)
    {
        var program = File.ReadAllText(GetRepositoryPath(programPath));

        program.Should().Contain("UseFastEndpoints(c => c.Endpoints.RoutePrefix = \"api\")");
    }

    [Theory]
    [InlineData("src/WebApi/Endpoints")]
    [InlineData("src/IdentityApi/Endpoints")]
    public void FastEndpoints_ShouldNotHardCodeApiRoutePrefix(string endpointsPath)
    {
        var endpointFiles = Directory.GetFiles(GetRepositoryPath(endpointsPath), "*.cs", SearchOption.AllDirectories);
        var filesWithHardCodedPrefix = endpointFiles
            .Where(path => HttpVerbRouteMethods.Any(method => File.ReadAllText(path).Contains($"{method}(\"/api", StringComparison.Ordinal)))
            .Select(path => Path.GetRelativePath(GetRepositoryPath("."), path))
            .ToArray();

        filesWithHardCodedPrefix.Should().BeEmpty();
    }

    private static string GetRepositoryPath(string relativePath)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "cpnucleo.slnx")))
        {
            directory = directory.Parent;
        }

        directory.Should().NotBeNull("the test should run from inside the cpnucleo repository output tree");

        return Path.Combine(directory!.FullName, relativePath);
    }

    private static int CountInvocationExpressions(string value, string methodName)
    {
        var root = CSharpSyntaxTree.ParseText(value).GetRoot();

        return root
            .DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .Count(invocation => invocation.Expression switch
            {
                IdentifierNameSyntax identifier => identifier.Identifier.ValueText == methodName,
                MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.ValueText == methodName,
                _ => false
            });
    }
}
