namespace Architecture.Tests;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;

public class FastEndpointsConfigurationTests
{
    private static readonly string[] HttpVerbRouteMethods = ["Get", "Post", "Put", "Delete", "Patch"];
    private static readonly string[] AuthenticationPackageNames = ["FastEndpoints.Security", "Microsoft.AspNetCore.Authentication.JwtBearer"];
    // The validation parameters themselves are covered behaviorally (Security.Unit.Tests and the
    // integration suite); this only pins that both API hosts use the shared configuration.
    private static readonly string[] JwtValidationSnippets =
    [
        "AddAuthentication(JwtBearerDefaults.AuthenticationScheme)",
        "options.MapInboundClaims = false",
        "options.MetadataAddress = JwtKeys.MetadataAddress(builder.Configuration)",
        "TokenValidationParameters = JwtKeys.ValidationParameters(builder.Configuration)",
        "OnTokenValidated",
        "GetRequiredService<TokenSessionValidator>()"
    ];

    [Fact]
    public void FastEndpointsPackageVersions_ShouldBeAligned()
    {
        // Versions are managed centrally; projects must not pin their own.
        var repositoryRoot = GetRepositoryPath(".");
        var pinnedInProjects = Directory
            .EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(path => IsSourceProject(repositoryRoot, path))
            .SelectMany(projectPath => XDocument.Load(projectPath).Descendants("PackageReference")
                .Where(x => x.Attribute("Version") is not null)
                .Select(x => $"{Path.GetRelativePath(repositoryRoot, projectPath)}: {x.Attribute("Include")?.Value}"))
            .ToArray();
        pinnedInProjects.Should().BeEmpty("package versions belong in Directory.Packages.props");

        var fastEndpointsVersions = XDocument.Load(GetRepositoryPath("src/Directory.Packages.props"))
            .Descendants("PackageVersion")
            .Concat(XDocument.Load(GetRepositoryPath("Directory.Packages.props")).Descendants("PackageVersion"))
            .Where(x => x.Attribute("Include")?.Value.StartsWith("FastEndpoints", StringComparison.Ordinal) == true)
            .Select(x => x.Attribute("Version")?.Value)
            .ToArray();

        fastEndpointsVersions.Should().NotBeEmpty();
        fastEndpointsVersions.Distinct().Should().ContainSingle("all FastEndpoints packages should use the same reviewed version")
            .Which.Should().Be("8.3.0");
    }

    [Fact]
    public void AuthenticationPackages_ShouldStayInIdentityAndApiHosts()
    {
        var repositoryRoot = GetRepositoryPath(".");
        var projectsWithAuthenticationPackages = Directory
            .EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(path => IsSourceProject(repositoryRoot, path))
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
            "src/WebApi/WebApi.csproj"
        ], "API hosts validate bearer tokens; IdentityApi issues them as an OpenID Connect server");

        var projectsWithIdentityServer = Directory
            .EnumerateFiles(repositoryRoot, "*.csproj", SearchOption.AllDirectories)
            .Where(path => IsSourceProject(repositoryRoot, path))
            .Where(projectPath => XDocument.Load(projectPath).Descendants("PackageReference")
                .Any(x => x.Attribute("Include")?.Value == "Open.IdentityServer"))
            .Select(projectPath => Path.GetRelativePath(repositoryRoot, projectPath))
            .ToArray();
        projectsWithIdentityServer.Should().BeEquivalentTo(["src/IdentityApi/IdentityApi.csproj"]);
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
        var useHealthChecksIndex = program.IndexOf("UseHealthChecks(\"/healthz\"", StringComparison.Ordinal);
        // IdentityApi authenticates inside UseIdentityServer() (identity cookie and protocol endpoints).
        var useAuthenticationIndex = program.IndexOf(programPath.Contains("IdentityApi", StringComparison.Ordinal) ? "UseIdentityServer()" : "UseAuthentication()", StringComparison.Ordinal);

        program.Should().Contain("AddCors(options =>");
        program.Should().Contain("CpnucleoWebClient");
        program.Should().Contain("Cors:AllowedOrigins");
        program.Should().Contain("https://cpnucleo.jonathanperis.tech");
        program.Should().Contain("AllowAnyHeader()");
        program.Should().Contain("AllowAnyMethod()");
        program.Should().Contain("UseRateLimiter()");
        useCorsIndex.Should().BeGreaterThanOrEqualTo(0);
        useHealthChecksIndex.Should().BeGreaterThan(useCorsIndex, "browser-run health checks call /healthz across subdomains and need CORS headers");
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
            : program.IndexOf(programPath.Contains("IdentityApi", StringComparison.Ordinal) ? "UseIdentityServer()" : "UseAuthentication()", StringComparison.Ordinal);

        program.Should().Contain("AddRateLimiter(options =>");
        program.Should().Contain("options.GlobalLimiter");
        program.Should().Contain("PartitionedRateLimiter.Create<HttpContext, string>");
        program.Should().Contain("RateLimitPartition.GetFixedWindowLimiter");
        program.Should().Contain("PermitLimit");
        program.Should().Contain("Window = TimeSpan.FromMinutes(1)");
        program.Should().Contain("QueueLimit");
        program.Should().Contain("context.Lease.TryGetMetadata(MetadataName.RetryAfter");
        program.Should().Contain("ApiErrorEnvelopeExtensions.WriteRateLimitRejectionAsync(");
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
        // WebApi takes bearer tokens; IdentityApi's own endpoints are the anonymous sign-in steps.
        program.Should().Contain(programPath.Contains("IdentityApi", StringComparison.Ordinal) ? "o.EnableJWTBearerAuth = false" : "o.EnableJWTBearerAuth = true");
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
    [InlineData("src/IdentityApi/appsettings.json", "api")]
    [InlineData("src/IdentityApi/appsettings.Development.json", "api")]
    [InlineData("src/WebApi/appsettings.json", "api")]
    [InlineData("src/WebApi/appsettings.Development.json", "api")]
    [InlineData("src/WebApi/appsettings.Testing.json", "api")]
    [InlineData("src/GrpcServer/appsettings.json", "grpc")]
    [InlineData("src/GrpcServer/appsettings.Development.json", "grpc")]
    public void AuthConfiguration_ShouldUseCpnucleoJonathanPerisTechDomains(string appSettingsPath, string audienceHost)
    {
        var appSettings = File.ReadAllText(GetRepositoryPath(appSettingsPath));

        appSettings.Should().Contain("\"Issuer\": \"https://identity-cpnucleo.jonathanperis.tech\"");
        // Each API host accepts only tokens issued for its own audience.
        appSettings.Should().Contain($"\"Audience\": \"https://{audienceHost}-cpnucleo.jonathanperis.tech\"");
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
    public void IdentityApi_ShouldBeAStandardOpenIdConnectServer()
    {
        var program = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Program.cs"));

        // Lifetimes and client rules are covered behaviorally (Security.Unit.Tests, integration suite).
        IdentityApi.Oidc.OidcSettings.AccessTokenLifetime.Should().Be(TimeSpan.FromMinutes(30));
        IdentityApi.Oidc.OidcSettings.MaximumSessionLength.Should().Be(TimeSpan.FromHours(8));
        program.Should().Contain(".AddIdentityServer(options =>");
        program.Should().Contain("app.UseIdentityServer();");
        program.Should().Contain(".AddPersistedGrantStore<PersistedGrantStore>()");
        program.Should().Contain("AddTransient<IRefreshTokenService, ReuseDetectingRefreshTokenService>()");
        program.Should().Contain("AddSingleton<ISigningCredentialStore>");
        program.Should().Contain("options.Authentication.CookieSlidingExpiration = false");
        program.Should().NotContain("AddDeveloperSigningCredential", "keys come from the encrypted key ring or a pinned PEM");
        Directory.Exists(GetRepositoryPath("src/IdentityApi/Endpoints/Refresh")).Should().BeFalse("refresh is the standard token endpoint grant");
    }

    [Fact]
    public void WebClient_ShouldExpireInactiveSessionsAndRefreshActiveTokens()
    {
        var httpClient = File.ReadAllText(GetRepositoryPath("src/WebClient/src/lib/api/http-client.ts"));
        var authGuard = File.ReadAllText(GetRepositoryPath("src/WebClient/src/components/AuthGuard.astro"));

        httpClient.Should().Contain("sessionInactivityTimeoutMs = 15 * 60 * 1000");
        httpClient.Should().Contain("tokenRefreshLeadMs = 5 * 60 * 1000");
        httpClient.Should().Contain("grant_type: 'refresh_token'");
        httpClient.Should().Contain("/connect/endsession");
        httpClient.Should().Contain("/connect/revocation");
        httpClient.Should().Contain("lastActivityStorageKey");
        httpClient.Should().Contain("setupSessionActivityTracking");
        httpClient.Should().Contain("redirectToLoginForExpiredSession()");
        httpClient.Should().Contain("BroadcastChannel", "logging out in one tab must sign out the other tabs");
        httpClient.Should().Contain("subscribeToCrossTabLogout(showSignedOut)", "a tab signed out elsewhere must not silently sign back in");
        httpClient.Should().Contain("const onInactivityTimeout", "the inactivity timer must re-check the last real user activity");
        authGuard.Should().Contain("setupSessionActivityTracking()");
        authGuard.Should().Contain("signOut()");
        authGuard.Should().Contain("redirectToLoginForExpiredSession()", "expired sessions also end the identity session");
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
            "builder.Configuration[\"OTEL_TRACES_SAMPLER\"]",
            "SetSampler(new ParentBasedSampler(new AlwaysOnSampler()))",
            "options.Filter = context => !IsHealthProbe(context.Request.Path)",
            "path.StartsWithSegments(\"/healthz\", StringComparison.OrdinalIgnoreCase)",
            "path.StartsWithSegments(\"/readyz\", StringComparison.OrdinalIgnoreCase)",
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

        telemetry.Should().NotContain("SetSampler(new AlwaysOnSampler())", "a hard-coded sampler would override OTEL_TRACES_SAMPLER");

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
        var themeToggle = File.ReadAllText(GetRepositoryPath("src/WebClient/src/components/ThemeToggle.astro"));
        var dashboard = File.ReadAllText(GetRepositoryPath("src/WebClient/src/pages/index.astro"));
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

        // Paper (light) is the default theme; the pre-paint script, the static markup and the toggle agree.
        appLayout.Should().Contain("<html lang=\"en\" data-theme=\"light\" style=\"color-scheme: light;\">");
        appLayout.Should().Contain(": 'light';");
        globalCss.Should().Contain("--accent: 58% 0.19 34;");
        globalCss.Should().Contain("--accent-hover: 52% 0.18 33;");
        globalCss.Should().Contain(":root[data-theme='dark']");
        globalCss.Should().Contain("scrollbar-color: oklch(var(--scrollbar-thumb)) oklch(var(--canvas));");
        globalCss.Should().Contain("scrollbar-width: thin;");
        globalCss.Should().Contain("::-webkit-scrollbar");
        globalCss.Should().Contain("::-webkit-scrollbar-thumb:hover { background: oklch(var(--accent-hover)); }");
        appLayout.Should().NotContain("Built as a clear place to review work, people, data, and releases without reading code first.");
        loginPage.Should().Contain("<html lang=\"en\" data-theme=\"light\" style=\"color-scheme: light;\">");
        loginPage.Should().Contain(": 'light';");
        themeToggle.Should().Contain("dataset.theme === 'dark'");
        themeToggle.Should().Contain(": 'dark';");
        dashboard.Should().NotContain("Dark by default · light-ready");

        // Renovate may pin the base image digest (node:26.9.0-alpine@sha256:...).
        dockerfile.Should().MatchRegex(@"FROM node:26\.9\.0-alpine(@sha256:[0-9a-f]{64})? AS runtime");
        dockerfile.Should().Contain("CMD [\"node\", \"scripts/preview.mjs\"]");

        compose.Should().Contain("OTEL_EXPORTER_OTLP_HTTP_ENDPOINT: http://otel-collector:4318");
        prodCompose.Should().Contain("OTEL_EXPORTER_OTLP_HTTP_ENDPOINT: ${OTEL_EXPORTER_OTLP_HTTP_ENDPOINT:-http://otel-collector:4318}");
        // PUBLIC_* URLs are compiled into the static assets by the release build;
        // runtime variables in production Compose would have no effect.
        prodCompose.Should().NotContain("PUBLIC_WEBAPI_BASE_URL:");
        prodCompose.Should().NotContain("PUBLIC_IDENTITY_API_BASE_URL:");
        prodCompose.Should().NotContain("PUBLIC_IDENTITY_API_ISSUER:");

        var releaseWorkflow = File.ReadAllText(GetRepositoryPath(".github/workflows/main-release.yml"));
        releaseWorkflow.Should().Contain("PUBLIC_WEBAPI_BASE_URL=https://api-cpnucleo.jonathanperis.tech/api");
        releaseWorkflow.Should().Contain("PUBLIC_IDENTITY_API_BASE_URL=https://identity-cpnucleo.jonathanperis.tech/api");
        releaseWorkflow.Should().Contain("PUBLIC_IDENTITY_API_ISSUER=https://identity-cpnucleo.jonathanperis.tech");
        releaseWorkflow.Should().NotContain("PUBLIC_IDENTITY_API_BASE_URL=http://localhost:5200/api");
    }

    [Fact]
    public void MainReleasePipeline_ShouldRunProductionSmokeTestsAfterHostingerDeploy()
    {
        var releaseWorkflow = File.ReadAllText(GetRepositoryPath(".github/workflows/main-release.yml"));
        var smokeScript = File.ReadAllText(GetRepositoryPath("scripts/smoke-production.sh"));

        releaseWorkflow.Should().Contain("- name: Run production smoke tests");
        releaseWorkflow.Should().Contain("run: scripts/smoke-production.sh");

        var deployIndex = releaseWorkflow.IndexOf("- name: Deploy Hostinger project", StringComparison.Ordinal);
        var smokeIndex = releaseWorkflow.IndexOf("- name: Run production smoke tests", StringComparison.Ordinal);
        deployIndex.Should().BeGreaterThanOrEqualTo(0);
        smokeIndex.Should().BeGreaterThan(deployIndex, "production smoke tests must run after the Hostinger deploy step");

        releaseWorkflow.Should().Contain("CPNUCLEO_WEB_URL: ${{ secrets.CPNUCLEO_WEB_URL }}");
        releaseWorkflow.Should().Contain("CPNUCLEO_API_URL: ${{ secrets.CPNUCLEO_API_URL }}");
        releaseWorkflow.Should().Contain("CPNUCLEO_IDENTITY_URL: ${{ secrets.CPNUCLEO_IDENTITY_URL }}");
        releaseWorkflow.Should().Contain("CPNUCLEO_GRPC_HEALTH_URL: ${{ secrets.CPNUCLEO_GRPC_HEALTH_URL }}");

        smokeScript.Should().Contain("for attempt in {1..10}");
        smokeScript.Should().Contain("--max-time 10");
        smokeScript.Should().Contain("sleep 5");
        smokeScript.Should().Contain("check_url \"WebClient\" \"${CPNUCLEO_WEB_URL:-}\" \"200,301,302\"");
        smokeScript.Should().Contain("check_url \"WebApi health\" \"${CPNUCLEO_API_URL%/}/healthz\" \"200\"");
        smokeScript.Should().Contain("check_url \"IdentityApi health\" \"${CPNUCLEO_IDENTITY_URL%/}/healthz\" \"200\"");
        smokeScript.Should().Contain("check_url \"Grpc health\" \"${CPNUCLEO_GRPC_HEALTH_URL}\" \"200\"");
    }

    [Fact]
    public void ApplicationContainers_ShouldCheckHealthzEveryMinute()
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

        compose.Should().Contain("interval: 1m");
        prodCompose.Should().Contain("interval: 1m");
        compose.Should().NotContain("interval: 10m");
        prodCompose.Should().NotContain("interval: 10m");
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

        apiDockerfile.Should().Contain("HEALTHCHECK --interval=1m");
        apiDockerfile.Should().Contain("GET /healthz HTTP/1.1");
        identityDockerfile.Should().Contain("HEALTHCHECK --interval=1m");
        identityDockerfile.Should().Contain("GET /healthz HTTP/1.1");
        grpcDockerfile.Should().Contain("HEALTHCHECK --interval=1m");
        grpcDockerfile.Should().Contain("GET /healthz HTTP/1.1");
        webClientDockerfile.Should().Contain("HEALTHCHECK --interval=1m");
        webClientDockerfile.Should().Contain("http://localhost:5030/healthz");
    }


    [Fact]
    public void SecurityAuditFindings_ShouldStayRemediated()
    {
        var deployWorkflow = File.ReadAllText(GetRepositoryPath(".github/workflows/deploy.yml"));
        var prodCompose = File.ReadAllText(GetRepositoryPath("compose.prod.yaml"));
        var webApiProgram = File.ReadAllText(GetRepositoryPath("src/WebApi/Program.cs"));
        var identityProgram = File.ReadAllText(GetRepositoryPath("src/IdentityApi/Program.cs"));
        var previewServer = File.ReadAllText(GetRepositoryPath("src/WebClient/scripts/preview.mjs"));

        TrackedFiles(".env").Should().BeEmpty("dotenv files with credentials must not be tracked (a local untracked .env is fine)");
        deployWorkflow.Should().Contain("pages-docs-deploy.yml@d7e3c753530db86cb01b9510ab045c99b172ba03");
        deployWorkflow.Should().NotContain("secrets: inherit");
        prodCompose.Should().NotContain("seed-csv-cpnucleo:");
        prodCompose.Should().Contain("cpnucleo-security-headers");

        foreach (var program in new[] { webApiProgram, identityProgram })
        {
            program.Should().Contain("Strict-Transport-Security");
            program.Should().Contain("X-Content-Type-Options");
            program.Should().Contain("X-Frame-Options");
            program.Should().Contain("Referrer-Policy");
            program.Should().Contain("Content-Security-Policy");
        }

        webApiProgram.Should().Contain("AddPolicy(\"UserAdministration\"");
        webApiProgram.Should().Contain("RequireClaim(CpnucleoClaimTypes.Subject)");
        previewServer.Should().Contain("securityHeaders");
        previewServer.Should().Contain("Content-Security-Policy");
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

    private static string[] TrackedFiles(string pathSpec)
    {
        using var git = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("git", ["ls-files", "--", pathSpec])
        {
            WorkingDirectory = GetRepositoryPath("."),
            RedirectStandardOutput = true,
            UseShellExecute = false
        })!;
        var output = git.StandardOutput.ReadToEnd();
        git.WaitForExit();
        git.ExitCode.Should().Be(0, "the architecture tests run inside the git checkout");
        return output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    }

    private static bool IsSourceProject(string repositoryRoot, string path) =>
        !Path.GetRelativePath(repositoryRoot, path)
            .Split(Path.DirectorySeparatorChar)
            .Any(segment => segment is "bin" or "obj" || segment.StartsWith('.'));

    private static string GetRepositoryPath(string relativePath)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Join(directory.FullName, "cpnucleo.slnx")))
        {
            directory = directory.Parent;
        }

        directory.Should().NotBeNull("the test should run from inside the cpnucleo repository output tree");

        return Path.Join(directory!.FullName, relativePath);
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
