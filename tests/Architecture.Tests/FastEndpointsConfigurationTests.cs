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
        program.Should().Contain("UseRateLimiter()");
        useCorsIndex.Should().BeGreaterThanOrEqualTo(0);
        useAuthenticationIndex.Should().BeGreaterThan(useCorsIndex, "CORS middleware must run before authentication/authorization so browser preflights are answered");
    }

    [Theory]
    [InlineData("src/WebApi/Program.cs")]
    [InlineData("src/IdentityApi/Program.cs")]
    [InlineData("src/GrpcServer/Program.cs")]
    public void ApiHosts_ShouldEnforceGlobalRateLimiting(string programPath)
    {
        var program = StripLineComments(File.ReadAllText(GetRepositoryPath(programPath)));
        var useRateLimiterIndex = program.IndexOf("UseRateLimiter()", StringComparison.Ordinal);
        var protectedPipelineIndex = programPath.Contains("GrpcServer", StringComparison.Ordinal)
            ? program.IndexOf("MapHandlers(h =>", StringComparison.Ordinal)
            : program.IndexOf("UseAuthentication()", StringComparison.Ordinal);

        program.Should().Contain("AddRateLimiter(options =>");
        program.Should().Contain("options.GlobalLimiter");
        program.Should().Contain("PartitionedRateLimiter.Create<HttpContext, string>");
        program.Should().Contain("RateLimitPartition.GetFixedWindowLimiter");
        program.Should().Contain("PermitLimit");
        program.Should().Contain("Window = TimeSpan.FromMinutes(1)");
        program.Should().Contain("QueueLimit");
        program.Should().Contain("Status429TooManyRequests");
        program.Should().Contain("Response.ContentType = \"text/plain\"");
        program.Should().Contain("context.Lease.TryGetMetadata(MetadataName.RetryAfter");
        program.Should().Contain("Headers.RetryAfter");
        program.Should().Contain("RequestServices");
        program.Should().Contain("GetRequiredService<ILoggerFactory>()");
        program.Should().Contain("LogWarning");
        program.Should().NotContain("LoggerFactory.Create(logging =>", "rate-limit rejection logs must use the host logging pipeline so OpenTelemetry receives them");
        useRateLimiterIndex.Should().BeGreaterThanOrEqualTo(0);
        protectedPipelineIndex.Should().BeGreaterThan(useRateLimiterIndex, "global rate limiting must run before authenticated API endpoints/handlers");
    }

    [Theory]
    [InlineData("src/WebApi/Program.cs", "Cpnucleo Web API")]
    [InlineData("src/IdentityApi/Program.cs", "Cpnucleo Identity API")]
    public void BrowserFacingApis_ShouldPublishRichSwaggerDocumentation(string programPath, string title)
    {
        var program = File.ReadAllText(GetRepositoryPath(programPath));

        program.Should().Contain(".SwaggerDocument(o =>");
        program.Should().Contain("o.EnableJWTBearerAuth = true");
        program.Should().Contain("o.ShortSchemaNames = true");
        program.Should().Contain("o.TagDescriptions");
        program.Should().Contain($"s.Title = \"{title}\"");
        program.Should().Contain("s.DocumentName = \"v1\"");
        program.Should().Contain("s.Description =");
        program.Should().Contain("s.Version = \"v1\"");
        program.Should().Contain("s.PostProcess = document =>");
        program.Should().Contain("document.Info.Contact");
        program.Should().Contain("document.Info.License");
        program.Should().Contain("document.Info.TermsOfService");
        program.Should().Contain("UseSwaggerGen();");
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

    [Fact]
    public void IdentityApi_ShouldIssueThirtyMinuteTokensAndRefreshAuthenticatedSessions()
    {
        var program = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Program.cs"));
        var loginEndpoint = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Endpoints/Login/Endpoint.cs"));
        var refreshEndpoint = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Endpoints/Refresh/Endpoint.cs"));

        program.Should().NotContain("o.ExpireAt = DateTime.UtcNow.AddMinutes(30)", "JWT expiry must be computed when each token is created, not once at startup");
        loginEndpoint.Should().Contain("o.ExpireAt = DateTime.UtcNow.AddMinutes(30)");
        refreshEndpoint.Should().Contain("o.ExpireAt = DateTime.UtcNow.AddMinutes(30)");
        refreshEndpoint.Should().Contain("Post(\"/refresh\")");
        refreshEndpoint.Should().Contain("JwtBearer.CreateToken(");
        refreshEndpoint.Should().NotContain("AllowAnonymous();", "refresh must require an authenticated bearer token");
    }

    [Fact]
    public void WebClient_ShouldExpireInactiveSessionsAndRefreshActiveTokens()
    {
        var httpClient = File.ReadAllText(GetRepositoryPath("src/WebClient/src/lib/api/http-client.ts"));
        var authGuard = File.ReadAllText(GetRepositoryPath("src/WebClient/src/components/auth-guard.tsx"));

        httpClient.Should().Contain("sessionInactivityTimeoutMs = 15 * 60 * 1000");
        httpClient.Should().Contain("tokenRefreshLeadMs = 5 * 60 * 1000");
        httpClient.Should().Contain("/refresh");
        httpClient.Should().Contain("lastActivityStorageKey");
        httpClient.Should().Contain("setupSessionActivityTracking");
        httpClient.Should().Contain("redirectToLoginForExpiredSession()");
        authGuard.Should().Contain("setupSessionActivityTracking()");
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
            "OpenTelemetry:IncludeExceptionDetails",
            "http.request.method",
            ".AddHttpClientInstrumentation(options =>",
            "SetSampler(new AlwaysOnSampler())",
            ".AddRuntimeInstrumentation()",
            ".AddProcessInstrumentation()",
            "Microsoft.AspNetCore.RateLimiting",
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "System.Net.Http",
            "System.Net.NameResolution",
            "Npgsql",
            ".AddNpgsql()",
            ".AddNpgsqlInstrumentation",
            "Logging.AddOpenTelemetry",
            "Logging.AddConsole()",
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
        var appLayout = File.ReadAllText(GetRepositoryPath("src/WebClient/src/layouts/AppLayout.astro"));
        var globalCss = File.ReadAllText(GetRepositoryPath("src/WebClient/src/global.css"));
        var loginPage = File.ReadAllText(GetRepositoryPath("src/WebClient/src/pages/login.astro"));
        var themeToggle = File.ReadAllText(GetRepositoryPath("src/WebClient/src/components/theme-toggle.tsx"));
        var dashboard = File.ReadAllText(GetRepositoryPath("src/WebClient/src/routes/index.tsx"));
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

        appLayout.Should().Contain("<html lang=\"en\" data-theme=\"dark\" style=\"color-scheme: dark;\">");
        appLayout.Should().Contain(": 'dark';");
        globalCss.Should().Contain("--accent: 78% 0.17 215;");
        globalCss.Should().Contain("--accent-hover: 83% 0.17 210;");
        globalCss.Should().Contain("scrollbar-color: oklch(var(--accent-hover)) oklch(var(--canvas));");
        globalCss.Should().Contain("scrollbar-width: thin;");
        globalCss.Should().Contain("::-webkit-scrollbar");
        globalCss.Should().Contain("::-webkit-scrollbar-thumb:hover { background: oklch(var(--accent-hover)); }");
        appLayout.Should().NotContain("Built as a clear place to review work, people, data, and releases without reading code first.");
        loginPage.Should().Contain("<html lang=\"en\" data-theme=\"dark\" style=\"color-scheme: dark;\">");
        loginPage.Should().Contain(": 'dark';");
        themeToggle.Should().Contain("useSignal<Theme>('dark')");
        themeToggle.Should().Contain(": 'dark';");
        dashboard.Should().Contain("Dark by default · light-ready");
        dashboard.Should().NotContain("Light by default · dark-ready");

        dockerfile.Should().Contain("FROM node:22-alpine AS runtime");
        dockerfile.Should().Contain("CMD [\"bun\", \"run\", \"preview\"]");

        compose.Should().Contain("OTEL_EXPORTER_OTLP_HTTP_ENDPOINT: http://otel-collector:4318");
        prodCompose.Should().Contain("OTEL_EXPORTER_OTLP_HTTP_ENDPOINT: http://otel-collector:4318");
        prodCompose.Should().Contain("PUBLIC_WEBAPI_BASE_URL: https://${CPNUCLEO_API_HOST:?Set CPNUCLEO_API_HOST}/api");
        prodCompose.Should().Contain("PUBLIC_IDENTITY_API_BASE_URL: https://${CPNUCLEO_IDENTITY_HOST:?Set CPNUCLEO_IDENTITY_HOST}/api");
        prodCompose.Should().Contain("PUBLIC_IDENTITY_API_ISSUER: https://${CPNUCLEO_IDENTITY_HOST:?Set CPNUCLEO_IDENTITY_HOST}");
        prodCompose.Should().NotContain("PUBLIC_IDENTITY_API_BASE_URL: http://localhost:5200");

        var releaseWorkflow = File.ReadAllText(GetRepositoryPath(".github/workflows/main-release.yml"));
        releaseWorkflow.Should().Contain("PUBLIC_WEBAPI_BASE_URL=https://api-cpnucleo.jonathanperis.tech/api");
        releaseWorkflow.Should().Contain("PUBLIC_IDENTITY_API_BASE_URL=https://identity-cpnucleo.jonathanperis.tech/api");
        releaseWorkflow.Should().Contain("PUBLIC_IDENTITY_API_ISSUER=https://identity-cpnucleo.jonathanperis.tech");
        releaseWorkflow.Should().NotContain("PUBLIC_IDENTITY_API_BASE_URL=http://localhost:5200/api");
    }

    [Fact]
    public void ApplicationContainers_ShouldCheckHealthzEveryTenMinutes()
    {
        var compose = File.ReadAllText(GetRepositoryPath("compose.yaml"));
        var prodCompose = File.ReadAllText(GetRepositoryPath("compose.prod.yaml"));
        var apiProgram = File.ReadAllText(GetRepositoryPath("src/WebApi/Program.cs"));
        var identityProgram = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Program.cs"));
        var grpcProgram = File.ReadAllText(GetRepositoryPath("src/GrpcServer/Program.cs"));
        var apiDockerfile = File.ReadAllText(GetRepositoryPath("src/WebApi/Dockerfile"));
        var identityDockerfile = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Dockerfile"));
        var grpcDockerfile = File.ReadAllText(GetRepositoryPath("src/GrpcServer/Dockerfile"));
        var webClientDockerfile = File.ReadAllText(GetRepositoryPath("src/WebClient/Dockerfile"));

        compose.Should().Contain("interval: 10m");
        prodCompose.Should().Contain("interval: 10m");
        compose.Should().Contain("start_period: 1m");
        prodCompose.Should().Contain("start_period: 1m");
        compose.Should().Contain("start_interval: 10s");
        prodCompose.Should().Contain("start_interval: 10s");
        compose.Should().Contain("/healthz");
        prodCompose.Should().Contain("/healthz");

        foreach (var program in new[] { apiProgram, identityProgram, grpcProgram })
        {
            program.Should().Contain("context.Request.Path.Value?.Equals(\"/healthz\", StringComparison.OrdinalIgnoreCase) == true");
            program.Should().Contain("app.Logger.LogInformation(\"GET /healthz {StatusCode}\", context.Response.StatusCode)");
        }

        apiDockerfile.Should().Contain("HEALTHCHECK --interval=10m");
        apiDockerfile.Should().Contain("GET /healthz HTTP/1.1");
        identityDockerfile.Should().Contain("HEALTHCHECK --interval=10m");
        identityDockerfile.Should().Contain("GET /healthz HTTP/1.1");
        grpcDockerfile.Should().Contain("HEALTHCHECK --interval=10m");
        grpcDockerfile.Should().Contain("GET /healthz HTTP/1.1");
        webClientDockerfile.Should().Contain("HEALTHCHECK --interval=10m");
        webClientDockerfile.Should().Contain("http://localhost:5030/healthz");
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

    private static string StripLineComments(string value)
    {
        return string.Join(Environment.NewLine, value
            .Split('\n')
            .Select(StripLineComment));
    }

    private static string StripLineComment(string line)
    {
        var inSingleQuote = false;
        var inDoubleQuote = false;
        var isEscaped = false;

        for (var i = 0; i < line.Length - 1; i++)
        {
            var current = line[i];

            if (isEscaped)
            {
                isEscaped = false;
                continue;
            }

            if (current == '\\' && (inSingleQuote || inDoubleQuote))
            {
                isEscaped = true;
                continue;
            }

            if (current == '\'' && !inDoubleQuote)
            {
                inSingleQuote = !inSingleQuote;
                continue;
            }

            if (current == '"' && !inSingleQuote)
            {
                inDoubleQuote = !inDoubleQuote;
                continue;
            }

            if (!inSingleQuote && !inDoubleQuote && current == '/' && line[i + 1] == '/')
            {
                return line[..i];
            }
        }

        return line;
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
