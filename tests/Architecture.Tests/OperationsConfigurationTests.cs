namespace Architecture.Tests;

using System.Text.RegularExpressions;

/// <summary>
/// Deployment, container and CI contracts that are easy to regress silently:
/// inlined production configs, per-service environments, container hardening,
/// image build rules and release supply-chain gates.
/// </summary>
public class OperationsConfigurationTests
{
    private static readonly string[] DotNetServices = ["WebApi", "IdentityApi", "GrpcServer"];

    public static TheoryData<string, string> InlinedProductionConfigs => new()
    {
        { "otel_collector_config", "otel-collector.yaml" },
        { "nginx_config", "nginx.conf" },
        { "init_track_commit_timestamp", "docker-entrypoint-initdb.d/001-track-commit-timestamp.sql" },
        { "init_database_dump_ddl", "docker-entrypoint-initdb.d/002-database-dump-ddl.sql" },
    };

    [Theory]
    [MemberData(nameof(InlinedProductionConfigs))]
    public void ProductionCompose_InlinedConfigs_ShouldMatchTheirSourceFiles(string configName, string sourcePath)
    {
        var configs = ReadInlinedConfigs(Read("compose.prod.yaml"));

        configs.Should().ContainKey(configName);
        var inlined = Normalize(configs[configName].Replace("$$", "$", StringComparison.Ordinal));
        var source = Normalize(Read(sourcePath));

        inlined.Should().Be(source, $"compose.prod.yaml configs.{configName} must stay byte-equal to {sourcePath} (Compose escapes $ as $$)");
    }

    [Fact]
    public void ProductionCompose_ShouldOnlyInlineConfigsWithKnownSources()
    {
        var known = InlinedProductionConfigs.Select(row => (string)row[0]).ToArray();

        ReadInlinedConfigs(Read("compose.prod.yaml")).Keys.Should().BeEquivalentTo(known);
    }

    [Fact]
    public void ProductionCompose_ShouldGiveEachContainerOnlyItsOwnEnvironment()
    {
        var compose = Read("compose.prod.yaml");
        var webClient = ServiceBlock(compose, "webclient-cpnucleo");
        var identity = ServiceBlock(compose, "identityapi-cpnucleo");
        var database = ServiceBlock(compose, "db");
        var runtimeEnv = AnchorBlock(compose, "x-dotnet-runtime-env");

        Regex.IsMatch(compose, @"(?m)^\s*env_file:").Should().BeFalse("containers must not receive the whole .env");
        webClient.Should().NotMatchRegex(@"PUBLIC_[A-Z_]+:", "WebClient URLs are compiled at build time and have no runtime effect");
        webClient.Should().Contain("OTEL_EXPORTER_OTLP_HTTP_ENDPOINT");
        webClient.Should().NotContain("DB_CONNECTION_STRING");
        webClient.Should().NotContain("Jwt__");
        runtimeEnv.Should().Contain("ASPNETCORE_FORWARDEDHEADERS_ENABLED: \"true\"");
        runtimeEnv.Should().Contain("Jwt__SigningPublicKey: ${Jwt__SigningPublicKey:-}");
        runtimeEnv.Should().NotContain("Jwt__SigningPrivateKey", "only the token issuer may hold the private key");
        runtimeEnv.Should().NotContain("GRAFANA");
        runtimeEnv.Should().NotContain("POSTGRES_PASSWORD");
        identity.Should().Contain("Jwt__SigningPrivateKey: ${Jwt__SigningPrivateKey:-}");
        runtimeEnv.Should().Contain("CPNUCLEO_ADMIN_LOGINS", "API hosts re-check admin claims against the configured logins");
        database.Should().NotContain("DB_CONNECTION_STRING");
        database.Should().NotContain("Jwt__");

        foreach (var service in new[] { "otel-lgtm" })
        {
            ServiceBlock(compose, service).Should().Contain("GF_SECURITY_ADMIN_PASSWORD");
        }

        foreach (var service in new[] { "webapi1-cpnucleo", "identityapi-cpnucleo", "grpcserver-cpnucleo", "webclient-cpnucleo", "nginx", "migrate-cpnucleo" })
        {
            ServiceBlock(compose, service).Should().NotContain("GRAFANA_ADMIN", $"{service} must not receive Grafana credentials");
        }
    }

    [Fact]
    public void ProductionCompose_ShouldHardenApplicationContainers()
    {
        var compose = Read("compose.prod.yaml");
        var hardening = AnchorBlock(compose, "x-app-hardening");
        var commonApi = AnchorBlock(compose, "x-common-api-prod");

        hardening.Should().Contain("no-new-privileges:true");
        hardening.Should().MatchRegex(@"cap_drop:\s*\n\s*- ALL");
        hardening.Should().Contain("read_only: true");
        hardening.Should().Contain("- /tmp");
        commonApi.Should().Contain("<<: *app-hardening");
        ServiceBlock(compose, "migrate-cpnucleo").Should().Contain("*app-hardening");
        ServiceBlock(compose, "webclient-cpnucleo").Should().Contain("*app-hardening");

        foreach (var service in new[] { "webapi1-cpnucleo", "identityapi-cpnucleo", "grpcserver-cpnucleo" })
        {
            ServiceBlock(compose, service).Should().Contain("*common-api-prod");
        }
    }

    [Fact]
    public void ProductionCompose_ShouldKeepOtlpOffTheTraefikNetwork()
    {
        var compose = Read("compose.prod.yaml");
        var lgtm = ServiceBlock(compose, "otel-lgtm");
        var collector = ServiceBlock(compose, "otel-collector");
        var nginx = ServiceBlock(compose, "nginx");

        lgtm.Should().NotContain("traefik", "OTLP 4317/4318 must not be reachable from the shared Traefik network");
        collector.Should().NotContain("traefik");
        nginx.Should().Contain("traefik.http.services.cpnucleo-grafana.loadbalancer.server.port=3000");
        nginx.Should().Contain("cpnucleo-grafana-auth");
        Read("nginx.conf").Should().Contain("server otel-lgtm:3000 resolve;");
    }

    [Fact]
    public void ProductionCompose_NginxShouldWaitForHealthyApis()
    {
        var nginx = ServiceBlock(Read("compose.prod.yaml"), "nginx");

        foreach (var api in new[] { "webapi1-cpnucleo", "webapi2-cpnucleo" })
        {
            nginx.Should().MatchRegex($@"{Regex.Escape(api)}:\s*\n\s*condition: service_healthy");
        }

        // NGINX never proxies IdentityApi, so an IdentityApi failure must not block the API gateway.
        nginx.Should().NotContain("identityapi-cpnucleo:");
    }

    [Fact]
    public void Nginx_ShouldReuseUpstreamConnectionsAndReResolveContainers()
    {
        var nginx = Read("nginx.conf");

        nginx.Should().NotContain("keepalive_timeout 0");
        nginx.Should().Contain("resolver 127.0.0.11 valid=10s");
        nginx.Should().Contain("zone api 64k;");
        nginx.Should().Contain("server webapi1-cpnucleo:5000 resolve;");
        nginx.Should().Contain("server webapi2-cpnucleo:5000 resolve;");
        nginx.Should().MatchRegex(@"upstream api \{[^}]*keepalive \d+;");
        nginx.Should().Contain("proxy_http_version 1.1;");
        nginx.Should().Contain("proxy_set_header Connection \"\";");
    }

    [Fact]
    public void LocalCollector_ShouldNotLabelTelemetryAsProduction()
    {
        var collector = Read("otel-collector.yaml");
        var prodCollector = ServiceBlock(Read("compose.prod.yaml"), "otel-collector");

        collector.Should().NotMatchRegex(@"value:\s*production");
        collector.Should().Contain("${env:CPNUCLEO_DEPLOYMENT_ENVIRONMENT:-development}");
        prodCollector.Should().Contain("CPNUCLEO_DEPLOYMENT_ENVIRONMENT: production");
    }

    [Fact]
    public void ComposeHealthchecks_ShouldUseToolsPresentInRuntimeImages()
    {
        foreach (var path in new[] { "compose.yaml", "compose.prod.yaml" })
        {
            var compose = Read(path);

            compose.Should().NotContain("\"curl\"", $"{path}: the runtime images do not ship curl");
            compose.Should().Contain("/dev/tcp/127.0.0.1/5000");
            compose.Should().Contain("\"wget\", \"-qO-\", \"http://127.0.0.1:5030/healthz\"");
        }
    }

    [Fact]
    public void LegacyCompose_ShouldMigrateBeforeStartingApis()
    {
        var compose = Read("compose.yaml");
        var migrator = ServiceBlock(compose, "migrate-cpnucleo");

        migrator.Should().Contain("command: [\"--migrate-database\"]");
        migrator.Should().MatchRegex(@"db:\s*\n\s*condition: service_healthy");
        AnchorBlock(compose, "x-common-api").Should().MatchRegex(@"migrate-cpnucleo:\s*\n\s*condition: service_completed_successfully");
    }

    [Theory]
    [InlineData("WebApi")]
    [InlineData("IdentityApi")]
    [InlineData("GrpcServer")]
    public void DotNetDockerfiles_ShouldPublishOnceWithoutBakedSecrets(string service)
    {
        var dockerfile = Read($"src/{service}/Dockerfile");
        var project = Read($"src/{service}/{service}.csproj");

        dockerfile.Should().NotContain("DB_CONNECTION_STRING", "connection strings are runtime secrets, never image layers");
        dockerfile.Should().NotContain("dotnet build", "a separate build compiles everything twice before publish");
        Regex.Matches(dockerfile, @"dotnet publish").Count.Should().Be(1);
        dockerfile.Should().Contain("FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:");
        dockerfile.Should().Contain("-r \"$(cat /tmp/rid)\"");
        dockerfile.Should().MatchRegex(@"if \[ ""\$\{AOT\}"" = ""true"" \]; then\s*\\\s*\n\s*apt-get update\s*\\\s*\n\s*&& apt-get install -y --no-install-recommends clang zlib1g-dev");
        dockerfile.Should().Contain("USER app");
        dockerfile.Should().Contain("HEALTHCHECK --interval=1m");
        dockerfile.Should().Contain("--start-period=1m --start-interval=10s");

        project.Should().Contain("<UseSystemResourceKeys>false</UseSystemResourceKeys>", "production exception messages must stay readable");
        project.Should().Contain("<EventSourceSupport>true</EventSourceSupport>");
        project.Should().Contain("<HttpActivityPropagationSupport>true</HttpActivityPropagationSupport>");
    }

    [Fact]
    public void WebClientDockerfile_ShouldDefaultToLocalUrlsAndRunWithoutBun()
    {
        var dockerfile = Read("src/WebClient/Dockerfile");
        var runtimeStage = dockerfile[dockerfile.IndexOf(" AS runtime", StringComparison.Ordinal)..];

        dockerfile.Should().Contain("ARG PUBLIC_WEBAPI_BASE_URL=http://localhost:5100/api");
        dockerfile.Should().Contain("ARG PUBLIC_IDENTITY_API_BASE_URL=http://localhost:5200/api");
        runtimeStage.Should().NotContain("bun", "the preview server is plain Node; Bun is a build-time tool");
        runtimeStage.Should().Contain("CMD [\"node\", \"scripts/preview.mjs\"]");
        runtimeStage.Should().Contain("USER node");
    }

    [Fact]
    public void Workflows_ShouldPinActionsAndBoundEveryJob()
    {
        foreach (var path in Directory.GetFiles(RepositoryPath(".github/workflows"), "*.yml"))
        {
            var workflow = File.ReadAllText(path);
            var name = Path.GetFileName(path);

            foreach (Match use in Regex.Matches(workflow, @"uses:\s*(\S+)"))
            {
                use.Groups[1].Value.Should().MatchRegex(@"@[0-9a-f]{40}$", $"{name}: {use.Groups[1].Value} must be pinned to a full commit SHA");
            }

            if (name is "main-release.yml" or "build-check.yml")
            {
                foreach (var (job, body) in Jobs(workflow))
                {
                    body.Should().Contain("timeout-minutes:", $"{name}: job {job} needs a timeout");
                }
            }
        }
    }

    [Fact]
    public void ReleaseWorkflow_ShouldTestNativeArm64ImagesBeforePublishingMutableTags()
    {
        var release = Read(".github/workflows/main-release.yml");
        var jobs = Jobs(release).ToDictionary(job => job.Name, job => job.Body);

        release.Should().NotContain("setup-qemu-action", "arm64 images are built on native arm64 runners");
        release.Should().NotContain("latest-arm64", "no mutable tag may be published before testing");
        release.Should().NotContain("fetch-depth: 0");
        release.Should().NotContain("sleep 30");
        release.Should().NotContain("cat > .env", "compose.lab.yaml never reads a generated .env");
        jobs["build-push-arm64"].Should().Contain("runs-on: ubuntu-24.04-arm");
        jobs["container-test-arm64"].Should().Contain("runs-on: ubuntu-24.04-arm");
        jobs["container-test-arm64"].Should().Contain("scripts/ci-container-test.sh");
        jobs["container-test"].Should().Contain("scripts/ci-container-test.sh");
        jobs["merge-manifest"].Should().MatchRegex(@"needs: \[[^\]]*container-test-arm64[^\]]*\]");
        jobs["merge-manifest"].Should().MatchRegex(@"needs: \[[^\]]*scan-amd64[^\]]*\]");

        foreach (var arch in new[] { "amd64", "arm64" })
        {
            var build = jobs[$"build-push-{arch}"];
            build.Should().Contain($"cache-from: type=gha,scope=${{{{ matrix.cache }}}}-{arch}");
            build.Should().Contain($"cache-to: type=gha,mode=max,scope=${{{{ matrix.cache }}}}-{arch}");
            build.Should().Contain("sbom: true");
            build.Should().Contain("provenance: mode=max");
            build.Should().MatchRegex($@"tags: \|\s*\n\s*\$\{{\{{ matrix.image \}}\}}:sha-\$\{{\{{ github.sha \}}\}}-{arch}\s*\n\s*platforms:");
        }
    }

    [Fact]
    public void ReleaseWorkflow_ShouldScanAndSignImagesWithScopedPermissions()
    {
        var release = Read(".github/workflows/main-release.yml");
        var jobs = Jobs(release).ToDictionary(job => job.Name, job => job.Body);

        jobs["scan-amd64"].Should().Contain("aquasecurity/trivy-action@");
        jobs["scan-amd64"].Should().Contain("security-events: write");
        jobs["scan-amd64"].Should().Contain("upload-sarif@");
        jobs["scan-amd64"].Should().Contain("GITHUB_STEP_SUMMARY");
        jobs["scan-amd64"].Should().MatchRegex(@"severity: CRITICAL\s*\n\s*ignore-unfixed: true\s*\n\s*exit-code: '1'");
        jobs["merge-manifest"].Should().Contain("sigstore/cosign-installer@");
        jobs["merge-manifest"].Should().Contain("cosign sign --yes");
        jobs["merge-manifest"].Should().Contain("id-token: write");
        jobs["merge-manifest"].Should().Contain("github.ref == 'refs/heads/main'", "only main may move mutable tags");
        jobs["merge-manifest"].Should().MatchRegex(@"needs: \[[^\]]*deploy-hostinger[^\]]*\]", "latest must not point at a release production rejected");
        foreach (var build in new[] { "build-push-amd64", "build-push-arm64" })
        {
            jobs[build].Should().Contain("cosign sign --yes", "the per-architecture images that are tested and deployed are signed");
            jobs[build].Should().Contain("id-token: write");
        }
        jobs["deploy-hostinger"].Should().Contain("cosign verify", "only signed images are deployed");
        jobs["deploy-hostinger"].Should().MatchRegex(@"needs: \[[^\]]*scan-amd64[^\]]*\]");
        jobs["deploy-hostinger"].Should().Contain("scripts/deploy-hostinger-docker-manager.sh --rollback");

        foreach (var (job, body) in jobs.Where(job => job.Key is not ("merge-manifest" or "build-push-amd64" or "build-push-arm64")))
        {
            body.Should().NotContain("id-token: write", $"{job} must not mint OIDC tokens");
        }

        foreach (var (job, body) in jobs.Where(job => job.Key is not "scan-amd64"))
        {
            body.Should().NotContain("security-events: write", $"{job} must not write code-scanning results");
        }
    }

    [Fact]
    public void BuildCheck_ShouldRunEachSuiteOnceAndReportCoverage()
    {
        var buildCheck = Read(".github/workflows/build-check.yml");
        var jobs = Jobs(buildCheck).ToDictionary(job => job.Name, job => job.Body);

        buildCheck.Should().Contain("cancel-in-progress: ${{ github.event_name == 'pull_request' }}");
        buildCheck.Should().NotContain("tests/Architecture.Tests", "Architecture tests run once inside the solution test run");
        buildCheck.Should().NotContain("# - name:", "stale commented-out steps must not linger");
        Regex.Matches(buildCheck, @"dotnet test ").Count.Should().Be(1);
        jobs["setup-build-test"].Should().Contain("--collect:\"XPlat Code Coverage\"");
        jobs["setup-build-test"].Should().Contain("danielpalme/ReportGenerator-GitHub-Action@");
        jobs["setup-build-test"].Should().Contain("SummaryGithub.md");
        jobs["coverage-upload"].Should().Contain("use_oidc: true");
        jobs["coverage-upload"].Should().Contain("id-token: write");
        jobs["setup-build-test"].Should().NotContain("id-token: write");
        jobs["release-image-build"].Should().Contain("TRIM=true");
        jobs["release-image-build"].Should().Contain("EXTRA_OPTIMIZE=true");
        jobs["release-image-build"].Should().Contain("push: false");
    }

    [Fact]
    public void BackupTooling_ShouldVerifyDumpsAndRestoreOnlyIntoDisposableContainers()
    {
        var backup = Read("scripts/backup-hostinger.sh");
        var verify = Read("scripts/verify-backup.sh");
        var prodPostgres = Regex.Match(Read("compose.prod.yaml"), @"image: (postgres:[0-9.]+)").Groups[1].Value;

        backup.Should().Contain("--env-file \"${ENV_FILE}\"");
        backup.Should().Contain("pg_restore --list");
        backup.Should().Contain("ps --status running --quiet db");
        backup.Should().Contain("BACKUP_REMOTE");
        backup.Should().NotContain("2>/dev/null");
        backup.IndexOf("pg_restore --list", StringComparison.Ordinal).Should().BeLessThan(backup.IndexOf("SHA256SUMS)", StringComparison.Ordinal));
        verify.Should().Contain("--network none");
        verify.Should().Contain("__EFMigrationsHistory");
        verify.Should().Contain($"VERIFY_IMAGE=\"{prodPostgres}\"", "restore checks must use the production PostgreSQL version");
    }

    private static Dictionary<string, string> ReadInlinedConfigs(string compose)
    {
        var lines = compose.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        var start = Array.IndexOf(lines, "configs:");
        start.Should().BeGreaterThanOrEqualTo(0, "compose.prod.yaml should declare top-level configs");

        var configs = new Dictionary<string, string>(StringComparer.Ordinal);
        for (var i = start + 1; i < lines.Length; i++)
        {
            if (lines[i].Length > 0 && !char.IsWhiteSpace(lines[i][0]))
            {
                break;
            }

            var name = Regex.Match(lines[i], @"^  ([A-Za-z0-9_-]+):\s*$");
            if (!name.Success)
            {
                continue;
            }

            lines[i + 1].Trim().Should().Be("content: |", $"configs.{name.Groups[1].Value} should use a literal block");
            var body = new List<string>();
            var j = i + 2;
            for (; j < lines.Length && (lines[j].Length == 0 || lines[j].StartsWith("      ", StringComparison.Ordinal)); j++)
            {
                body.Add(lines[j].Length == 0 ? string.Empty : lines[j][6..]);
            }

            configs[name.Groups[1].Value] = string.Join('\n', body);
            i = j - 1;
        }

        return configs;
    }

    // A service is "  name:" (optionally followed by an anchor); its body is every
    // following line indented deeper than two spaces.
    private static string ServiceBlock(string compose, string service) =>
        IndentedBlock(compose, line => line == $"  {service}:" || line.StartsWith($"  {service}: ", StringComparison.Ordinal), "  ", service);

    // An extension field is "x-name: &anchor" at column zero.
    private static string AnchorBlock(string compose, string anchor) =>
        IndentedBlock(compose, line => line.StartsWith($"{anchor}: &", StringComparison.Ordinal), string.Empty, anchor);

    private static string IndentedBlock(string text, Func<string, bool> isHeader, string headerIndent, string description)
    {
        var lines = text.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        var start = Array.FindIndex(lines, line => isHeader(line));
        start.Should().BeGreaterThanOrEqualTo(0, $"'{description}' should exist");

        var body = new List<string> { lines[start] };
        for (var i = start + 1; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.Trim().Length > 0 && !line.StartsWith(headerIndent + " ", StringComparison.Ordinal))
            {
                break;
            }

            body.Add(line);
        }

        return string.Join('\n', body);
    }

    private static IEnumerable<(string Name, string Body)> Jobs(string workflow)
    {
        var lines = workflow.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        var start = Array.IndexOf(lines, "jobs:");
        start.Should().BeGreaterThanOrEqualTo(0);

        string? name = null;
        var body = new List<string>();
        foreach (var line in lines.Skip(start + 1))
        {
            var header = Regex.Match(line, @"^  ([A-Za-z0-9_-]+):\s*$");
            if (header.Success)
            {
                if (name is not null)
                {
                    yield return (name, string.Join('\n', body));
                }

                name = header.Groups[1].Value;
                body.Clear();
                continue;
            }

            if (line.Length > 0 && !char.IsWhiteSpace(line[0]))
            {
                break;
            }

            body.Add(line);
        }

        if (name is not null)
        {
            yield return (name, string.Join('\n', body));
        }
    }

    private static string Normalize(string value) =>
        value.TrimStart('﻿').Replace("\r\n", "\n", StringComparison.Ordinal).TrimEnd('\n', ' ');

    private static string Read(string relativePath) => File.ReadAllText(RepositoryPath(relativePath));

    private static string RepositoryPath(string relativePath)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "cpnucleo.slnx")))
        {
            directory = directory.Parent;
        }

        directory.Should().NotBeNull("the test should run from inside the cpnucleo repository output tree");

        return Path.Combine(directory!.FullName, relativePath);
    }
}
