using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Domain.Common.Security;
using Infrastructure.Common.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

// Native AOT compatibility experiment for the real WebApi host. It publishes WebApi with the
// existing opt-in -p:AOT=true switch (PublishAot), summarizes the trim/AOT diagnostics, then starts
// the native binary (and a JIT publish of the same commit as a baseline) against a disposable
// PostgreSQL container and probes it over HTTP. Everything reported is an observation of this run.
// A third variant applies FastEndpoints' Native AOT guidance to a scratch copy of src/ (never to
// src/ itself) to see how far the cheap steps get.
// Exit codes: 0 = the experiment ran (whatever it observed); 2 = the publish tooling could not run.

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

string? Option(string key) => Array.IndexOf(args, key) is var i and >= 0 && i + 1 < args.Length ? args[i + 1] : null;
bool Flag(string key) => args.Contains(key);

var repoRoot = FindRepoRoot();
var rid = Option("--rid") ?? RuntimeInformation.RuntimeIdentifier;
var dotnet = Option("--dotnet") ?? ResolveDotnet();
var skipJit = Flag("--skip-jit-baseline");
var skipGuided = Flag("--skip-guided");
var keepArtifacts = Flag("--keep-artifacts");
var singleWarn = Flag("--single-warn"); // default ILC behaviour: one IL2104/IL3053 per package assembly
var stamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
// Canonical path: on macOS the temp folder sits behind the /var -> /private/var symlink, and NuGet
// restore would otherwise see the copied project references under two different paths.
var work = Path.Join(RealPath(Path.GetTempPath()), "cpnucleo-nativeaot-lab", stamp);
var reportDir = Path.Join(AppContext.BaseDirectory, "reports", stamp);
Directory.CreateDirectory(work);
Directory.CreateDirectory(reportDir);
var totalWatch = Stopwatch.StartNew();
var webApiProject = Path.Join(repoRoot, "src", "WebApi", "WebApi.csproj");

Console.WriteLine($"Native AOT lab: dotnet={dotnet} rid={rid}");
Console.WriteLine($"Scratch: {work}");
Console.WriteLine($"Report:  {reportDir}");

var sdkVersion = await TryRunAsync(dotnet, ["--version"], repoRoot);
if (sdkVersion is null)
{
    Console.Error.WriteLine($"The publish tooling could not run: '{dotnet} --version' failed.");
    return 2;
}
var commit = await TryRunAsync("git", ["rev-parse", "--short", "HEAD"], repoRoot) ?? "unknown";

// 1. Native AOT publish (the experiment) and a JIT publish of the same sources (the baseline).
// FastEndpoints.Swagger's build targets set SuppressTrimAnalysisWarnings/SuppressAotAnalysisWarnings
// to true in the consuming project, which hides almost every diagnostic of a plain -p:AOT=true
// publish. Global properties win over project properties, so the lab turns them back on
// (--as-is keeps the suppression, to see what the Dockerfile's AOT build reports).
var asIs = Flag("--as-is");
var swaggerSuppression = FindSwaggerSuppression();
string[] aotArguments =
[
    "-p:AOT=true",
    .. asIs ? Array.Empty<string>() : ["-p:SuppressTrimAnalysisWarnings=false", "-p:SuppressAotAnalysisWarnings=false"],
    .. singleWarn ? Array.Empty<string>() : ["-p:TrimmerSingleWarn=false"]
];
var native = await PublishAsync("native-aot", webApiProject, [.. aotArguments, "-p:EmitCompilerGeneratedFiles=true"]);
if (native.ToolingFailed)
{
    Console.Error.WriteLine("The publish tooling itself could not run (it failed before native compilation started). See " + native.LogPath);
    return 2;
}
var jit = skipJit ? null : await PublishAsync("jit-baseline", webApiProject, ["--self-contained", "false"]);

// FastEndpoints guidance on a scratch copy: source-generated discovery (DiscoveredTypes.All), the
// generated binding reflection cache, and reflection-based System.Text.Json re-enabled as a stand-in
// for generated serializer contexts (those need FastEndpoints' CLI tool and committed sources).
(string? guidedProject, List<string> guidedPatches) = skipGuided ? (null, []) : PrepareGuidedCopy();
var guided = guidedProject is null ? null
    : await PublishAsync("native-aot-fe-guidance", guidedProject, [.. aotArguments, "-p:JsonSerializerIsReflectionEnabledByDefault=true"]);
List<IlWarning> guidedWarnings = guided is null ? [] : WarningSummary.Parse(guided.Output);

// 2. Warning summary.
var warnings = WarningSummary.Parse(native.Output);
var generated = InspectGeneratedSources(Path.Join(work, "native-aot", "artifacts"));

// 3. Run what was produced against a disposable database.
var probes = new Dictionary<string, RunResult>();
string migrationNote = "not attempted (no binary was produced)";
if (new[] { native, guided, jit }.Any(p => p?.Binary is not null))
{
    await using var database = new PostgreSqlBuilder("postgres:16.15").WithCommand("-c", "track_commit_timestamp=on").Build();
    await database.StartAsync();
    var connectionString = database.GetConnectionString();

    // EF Core migrations are applied by the JIT lab process so readiness can be probed even if the
    // native migrator fails. The native "--migrate-database" path is tried on a separate database.
    await using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(connectionString).Options))
        await context.Database.MigrateAsync();
    var latestMigration = DatabaseReadinessCheck.LatestMigrationId;

    const string login = "aot-lab@cpnucleo.local";
    const string passwordHash = "not-a-real-hash-native-aot-lab";
    var userId = Guid.CreateVersion7();
    await using (var connection = new NpgsqlConnection(connectionString))
    {
        await connection.OpenAsync();
        await using var insert = new NpgsqlCommand("""
            INSERT INTO "Users" ("Id", "Name", "Login", "Password", "Salt", "CreatedAt", "Active")
            VALUES (@id, @login, @login, @password, '', now(), true)
            """, connection);
        insert.Parameters.AddWithValue("id", userId);
        insert.Parameters.AddWithValue("login", login);
        insert.Parameters.AddWithValue("password", passwordHash);
        await insert.ExecuteNonQueryAsync();
        // CREATE DATABASE cannot share a batch (implicit transaction) with other statements.
        await using var create = new NpgsqlCommand("CREATE DATABASE native_migrate_probe", connection);
        await create.ExecuteNonQueryAsync();
    }

    if (native.Binary is not null)
    {
        var probeDb = new NpgsqlConnectionStringBuilder(connectionString) { Database = "native_migrate_probe" }.ConnectionString;
        var (exit, output, elapsed) = await RunToExitAsync(native.Binary, ["--migrate-database"], native.PublishDir,
            new() { ["DB_CONNECTION_STRING"] = probeDb, ["ASPNETCORE_ENVIRONMENT"] = "Production" }, TimeSpan.FromMinutes(3));
        File.WriteAllLines(Path.Join(reportDir, "native-migrate.log"), output);
        bool applied;
        try
        {
            await using var check = new NpgsqlConnection(probeDb);
            await check.OpenAsync();
            await using var command = new NpgsqlCommand("""SELECT EXISTS (SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = @id)""", check);
            command.Parameters.AddWithValue("id", latestMigration);
            applied = (bool)(await command.ExecuteScalarAsync() ?? false);
        }
        catch (PostgresException) { applied = false; }
        migrationNote = $"native `--migrate-database` exit code {exit?.ToString() ?? "timeout"} after {elapsed.TotalSeconds:F1}s; latest migration `{latestMigration}` applied: **{(applied ? "yes" : "no")}**"
            + (applied ? "" : $". First error: `{Shorten(FirstError(output), 300).Replace('`', '\'')}`");
    }

    // A stand-in OpenID Connect discovery document + JWKS, so the API host can validate a token
    // shaped like IdentityApi's (RS256, typ at+jwt) without starting IdentityApi.
    using var rsa = RSA.Create(2048);
    const string issuer = "https://identity.nativeaot.lab";
    const string audience = "https://api.nativeaot.lab";
    var keyId = Convert.ToHexString(SHA256.HashData(rsa.ExportSubjectPublicKeyInfo()))[..16];
    var discoveryPort = FreePort();
    using var discovery = new HttpListener();
    discovery.Prefixes.Add($"http://127.0.0.1:{discoveryPort}/");
    discovery.Start();
    using var discoveryStop = new CancellationTokenSource();
    var discoveryTask = ServeDiscoveryAsync(discovery, issuer, $"http://127.0.0.1:{discoveryPort}/jwks", rsa, keyId, discoveryStop.Token);
    var token = MintToken(rsa, keyId, issuer, audience, userId, login, SecurityStamp.Compute(passwordHash, login));

    var environment = new Dictionary<string, string>
    {
        ["ASPNETCORE_ENVIRONMENT"] = "Production",
        ["DB_CONNECTION_STRING"] = connectionString,
        ["Jwt__Issuer"] = issuer,
        ["Jwt__Audience"] = audience,
        ["Jwt__MetadataAddress"] = $"http://127.0.0.1:{discoveryPort}/.well-known/openid-configuration",
        ["CPNUCLEO_ADMIN_LOGINS"] = login,
        // No collector runs; the exporter keeps failing quietly, as with an unreachable collector.
        ["OTEL_EXPORTER_OTLP_ENDPOINT"] = "http://127.0.0.1:9"
    };
    if (native.Binary is not null) probes["native-aot"] = await ProbeAsync("native-aot", native, environment, token);
    if (guided?.Binary is not null) probes[guided.Variant] = await ProbeAsync(guided.Variant, guided, environment, token);
    if (jit?.Binary is not null) probes["jit-baseline"] = await ProbeAsync("jit-baseline", jit, environment, token);

    discoveryStop.Cancel();
    discovery.Stop();
    try { await discoveryTask; }
    catch (Exception e) when (e is OperationCanceledException or HttpListenerException or ObjectDisposedException)
    {
        // Expected: stopping the discovery listener ends its accept loop.
    }
}

// 4. Report.
var report = BuildReport();
var reportPath = Path.Join(reportDir, "native-aot-report.md");
await File.WriteAllTextAsync(reportPath, report);
Console.WriteLine();
Console.WriteLine(report);
Console.WriteLine($"Report written to {reportPath} (full publish logs alongside it).");

if (!keepArtifacts)
{
    try { Directory.Delete(work, recursive: true); }
    catch (IOException e) { Console.WriteLine($"Could not delete scratch folder {work}: {e.Message}"); }
}
else Console.WriteLine($"Publish artifacts kept in {work}");
return 0;

// ---------------------------------------------------------------------------------------------

async Task<PublishResult> PublishAsync(string variant, string project, string[] extra)
{
    var publishDir = Path.Join(work, variant, "publish");
    string[] arguments =
    [
        "publish", project, "-c", "Release", "-r", rid, "-o", publishDir,
        // Keeps bin/obj out of src/ so the experiment never disturbs the developer's build.
        "--artifacts-path", Path.Join(work, variant, "artifacts"),
        "-tl:off", "-clp:NoSummary", "-nologo", .. extra
    ];
    Console.WriteLine($"[{variant}] dotnet {string.Join(' ', arguments)}");
    var watch = Stopwatch.StartNew();
    var (exit, output, _) = await RunToExitAsync(dotnet, arguments, repoRoot, [], TimeSpan.FromMinutes(30), echo: variant);
    watch.Stop();
    var logPath = Path.Join(reportDir, $"publish-{variant}.log");
    await File.WriteAllLinesAsync(logPath, output);
    var binaryPath = Path.Join(publishDir, OperatingSystem.IsWindows() ? "WebApi.exe" : "WebApi");
    var binary = exit == 0 && File.Exists(binaryPath) ? binaryPath : null;
    var nativeStarted = output.Any(line => line.Contains("Generating native code", StringComparison.Ordinal) || line.Contains("ILC", StringComparison.Ordinal));
    var toolingFailed = exit is null || (exit != 0 && variant == "native-aot" && !nativeStarted);
    Console.WriteLine($"[{variant}] exit {exit?.ToString() ?? "timeout"} in {watch.Elapsed.TotalSeconds:F0}s; binary: {binary ?? "none"}");
    return new PublishResult(variant, string.Join(' ', arguments), exit, watch.Elapsed, output, logPath, publishDir, binary, toolingFailed);
}

async Task<RunResult> ProbeAsync(string variant, PublishResult publish, Dictionary<string, string> environment, string token)
{
    var port = FreePort();
    var output = new List<string>();
    var start = new ProcessStartInfo(publish.Binary!) { WorkingDirectory = publish.PublishDir, RedirectStandardOutput = true, RedirectStandardError = true };
    foreach (var (key, value) in environment) start.Environment[key] = value;
    start.Environment["ASPNETCORE_URLS"] = $"http://127.0.0.1:{port}";
    var watch = Stopwatch.StartNew();
    using var process = Process.Start(start)!;
    process.OutputDataReceived += (_, e) => { if (e.Data is not null) lock (output) output.Add(e.Data); };
    process.ErrorDataReceived += (_, e) => { if (e.Data is not null) lock (output) output.Add(e.Data); };
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    using var http = new HttpClient { BaseAddress = new Uri($"http://127.0.0.1:{port}"), Timeout = TimeSpan.FromSeconds(15) };
    var results = new List<ProbeResult>();
    TimeSpan? startup = null;
    try
    {
        while (watch.Elapsed < TimeSpan.FromSeconds(60) && !process.HasExited)
        {
            try
            {
                using var ready = await http.GetAsync("/healthz");
                if (ready.StatusCode == HttpStatusCode.OK) { startup = watch.Elapsed; break; }
            }
            catch (HttpRequestException)
            {
                // Not listening yet; poll again.
            }
            await Task.Delay(25);
        }
        if (startup is not null)
        {
            var organizationId = Guid.CreateVersion7();
            var assignmentTypeId = Guid.CreateVersion7();
            (string Name, HttpMethod Method, string Path, bool Auth, string? Body, int Expected, string Check)[] plan =
            [
                ("liveness", HttpMethod.Get, "/healthz", false, null, 200, ""),
                ("readiness (PostgreSQL + newest migration)", HttpMethod.Get, "/readyz", false, null, 200, ""),
                ("OpenAPI document", HttpMethod.Get, "/swagger/v1/swagger.json", false, null, 200, "\"paths\""),
                ("root minimal API", HttpMethod.Get, "/", false, null, 200, "Hello World!"),
                ("anonymous /api/projects (error envelope)", HttpMethod.Get, "/api/projects", false, null, 401, "statusCode"),
                ("list projects (Dapper basic repository)", HttpMethod.Get, "/api/projects?pageSize=5", true, null, 200, "\"result\""),
                ("create organization (Dapper unit of work)", HttpMethod.Post, "/api/organization", true, $$"""{"id":"{{organizationId}}","name":"AOT lab","description":"native probe"}""", 200, organizationId.ToString()),
                ("create assignment type (EF Core)", HttpMethod.Post, "/api/assignmentType", true, $$"""{"id":"{{assignmentTypeId}}","name":"AOT lab"}""", 200, assignmentTypeId.ToString()),
                ("list organizations (Dapper unit of work)", HttpMethod.Get, "/api/organizations?pageSize=5", true, null, 200, organizationId.ToString()),
                ("invalid paging (validation envelope)", HttpMethod.Get, "/api/projects?pageSize=0", true, null, 400, "statusCode")
            ];
            foreach (var step in plan)
            {
                using var request = new HttpRequestMessage(step.Method, step.Path);
                if (step.Auth) request.Headers.Authorization = new("Bearer", token);
                if (step.Body is not null) request.Content = new StringContent(step.Body, Encoding.UTF8, "application/json");
                var probeWatch = Stopwatch.StartNew();
                try
                {
                    using var response = await http.SendAsync(request);
                    var body = await response.Content.ReadAsStringAsync();
                    var ok = (int)response.StatusCode == step.Expected && (step.Check.Length == 0 || body.Contains(step.Check, StringComparison.Ordinal));
                    results.Add(new ProbeResult(step.Name, $"{step.Method} {step.Path}", step.Expected, (int)response.StatusCode, ok, probeWatch.Elapsed, ok ? "" : Shorten(body)));
                }
                catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
                {
                    results.Add(new ProbeResult(step.Name, $"{step.Method} {step.Path}", step.Expected, null, false, probeWatch.Elapsed, e.GetType().Name + ": " + Shorten(e.Message)));
                }
            }
        }
        var rss = process.HasExited ? null : await TryRunAsync("ps", ["-o", "rss=", "-p", process.Id.ToString()], repoRoot);
        var exited = process.HasExited ? process.ExitCode : (int?)null;
        return new RunResult(variant, startup, long.TryParse(rss, out var kb) ? kb * 1024 : null, exited, results, Snapshot());
    }
    finally
    {
        if (!process.HasExited) { process.Kill(entireProcessTree: true); await process.WaitForExitAsync(); }
        await File.WriteAllLinesAsync(Path.Join(reportDir, $"run-{variant}.log"), Snapshot());
    }

    List<string> Snapshot() { lock (output) return [.. output]; }
}

string BuildReport()
{
    var sb = new StringBuilder();
    sb.AppendLine("# Native AOT compatibility experiment: WebApi");
    sb.AppendLine();
    sb.AppendLine("All results below are **observations of one local run**, not guarantees. WebApi is not claimed to be Native AOT compatible.");
    sb.AppendLine();
    sb.AppendLine($"- Date (UTC): {DateTime.UtcNow:yyyy-MM-dd HH:mm}");
    sb.AppendLine($"- OS: {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture}); target RID `{rid}`");
    sb.AppendLine($"- SDK: {sdkVersion} (`{dotnet}`); commit `{commit}`");
    sb.AppendLine($"- Lab runtime: {totalWatch.Elapsed:mm\\:ss} (mm:ss)");
    sb.AppendLine();
    sb.AppendLine("## Publish");
    sb.AppendLine();
    sb.AppendLine("| Variant | Exit | Duration | Binary | Binary size | Publish folder (no symbols) |");
    sb.AppendLine("|---|---|---|---|---|---|");
    foreach (var p in new[] { native, guided, jit }.OfType<PublishResult>())
        sb.AppendLine($"| {p.Variant} | {p.ExitCode?.ToString() ?? "timeout"} | {p.Duration.TotalSeconds:F0}s | {(p.Binary is null ? "not produced" : "yes")} | {(p.Binary is null ? "-" : Mb(new FileInfo(p.Binary).Length))} | {(Directory.Exists(p.PublishDir) ? Mb(FolderSize(p.PublishDir)) : "-")} |");
    sb.AppendLine();
    sb.AppendLine($"Native command: `dotnet {native.Command.Replace(work, "<scratch>").Replace(repoRoot, "<repo>")}`");
    sb.AppendLine();
    foreach (var p in new[] { native, guided, jit }.OfType<PublishResult>())
    {
        var errors = p.Output.Where(line => line.Contains(": error ", StringComparison.Ordinal)).Distinct().Take(10).ToList();
        if (errors.Count == 0) continue;
        sb.AppendLine($"Publish errors of `{p.Variant}` (first 10):");
        sb.AppendLine();
        foreach (var error in errors) sb.AppendLine($"- `{Shorten(error.Replace(repoRoot, "<repo>").Replace(work, "<scratch>"), 300)}`");
        sb.AppendLine();
    }

    sb.AppendLine("## Trim (IL2xxx) and AOT (IL3xxx) diagnostics of the native publish");
    sb.AppendLine();
    sb.AppendLine(singleWarn
        ? "ILC default mode: package assemblies are collapsed into one IL2104/IL3053 each."
        : "Detailed mode (`-p:TrimmerSingleWarn=false`): each warning is attributed to the member raising it (the caller), grouped by its package family. Duplicates are removed.");
    sb.AppendLine();
    sb.AppendLine(asIs
        ? $"`--as-is`: the project's own warning suppression was kept. FastEndpoints.Swagger build targets: {swaggerSuppression}."
        : $"Suppression overridden with `-p:SuppressTrimAnalysisWarnings=false -p:SuppressAotAnalysisWarnings=false`. FastEndpoints.Swagger build targets: {swaggerSuppression}. A plain `-p:AOT=true` publish (as in the Dockerfile) therefore shows almost none of the diagnostics below; rerun with `--as-is` to compare.");
    sb.AppendLine();
    sb.AppendLine($"Unique diagnostics: **{warnings.Count}** ({warnings.Count(w => w.Code.StartsWith("IL2"))} trim, {warnings.Count(w => w.Code.StartsWith("IL3"))} AOT, {warnings.Count(w => w.IsError)} reported as errors).");
    sb.AppendLine();
    sb.AppendLine("| Code | Count | Meaning |");
    sb.AppendLine("|---|---|---|");
    foreach (var group in warnings.GroupBy(w => w.Code).OrderByDescending(g => g.Count()).ThenBy(g => g.Key))
        sb.AppendLine($"| {group.Key} | {group.Count()} | {WarningSummary.Meaning(group.Key)} |");
    sb.AppendLine();
    sb.AppendLine("| Origin (package family of the raising member) | Count | Codes |");
    sb.AppendLine("|---|---|---|");
    foreach (var group in warnings.GroupBy(w => w.Origin).OrderByDescending(g => g.Count()).ThenBy(g => g.Key))
        sb.AppendLine($"| {group.Key} | {group.Count()} | {string.Join(", ", group.GroupBy(w => w.Code).OrderByDescending(c => c.Count()).Select(c => $"{c.Key}×{c.Count()}"))} |");
    sb.AppendLine();
    var samples = warnings.Where(w => !w.Origin.StartsWith("Cpnucleo", StringComparison.Ordinal))
        .GroupBy(w => w.Origin).OrderByDescending(g => g.Count()).Take(12).ToList();
    if (samples.Count > 0)
    {
        sb.AppendLine("Sample per package family (first two distinct, shortened; full text in the publish log):");
        sb.AppendLine();
        foreach (var group in samples)
            foreach (var w in group.DistinctBy(w => w.Code + w.Member).Take(2))
                sb.AppendLine($"- {group.Key}, {w.Code}: {Shorten(w.Message.Replace('`', '\''), 220)}");
        sb.AppendLine();
    }
    var own = warnings.Where(w => w.Origin.StartsWith("Cpnucleo", StringComparison.Ordinal)).ToList();
    if (own.Count > 0)
    {
        sb.AppendLine("Diagnostics raised in this repository's code (first 15):");
        sb.AppendLine();
        foreach (var w in own.Take(15)) sb.AppendLine($"- {w.Code} `{Shorten(w.Member, 120)}`: {Shorten(w.Message.Replace(repoRoot, "<repo>"), 220)}");
        sb.AppendLine();
    }

    sb.AppendLine("## FastEndpoints Native AOT guidance vs this tree");
    sb.AppendLine();
    sb.AppendLine("Checked statically (the lab does not change `src/`). Guidance: https://fast-endpoints.com/docs/native-aot");
    sb.AppendLine();
    var program = File.ReadAllText(Path.Join(repoRoot, "src", "WebApi", "Program.cs"));
    var csproj = File.ReadAllText(webApiProject);
    sb.AppendLine("| Guidance | Observed |");
    sb.AppendLine("|---|---|");
    sb.AppendLine($"| `WebApplication.CreateSlimBuilder` | {(program.Contains("CreateSlimBuilder") ? "used" : "not used")} |");
    sb.AppendLine($"| `AddFastEndpoints(DiscoveredTypes.All)` (source-generated discovery) | {(program.Contains("DiscoveredTypes") ? "used" : "not used: `AddFastEndpoints` scans the WebApi assembly (reflection)")}; generator output `DiscoveredTypes`: {generated.DiscoveredTypes} |");
    sb.AppendLine($"| `GenerateSerializerContexts` + `AddSerializerContextsFrom...` (committed `JsonSerializerContext`s) | {(csproj.Contains("GenerateSerializerContexts") || program.Contains("AddSerializerContextsFrom") ? "present" : "absent: request/response DTOs rely on reflection-based System.Text.Json, which Native AOT disables by default")} |");
    sb.AppendLine($"| `Binding.ReflectionCache.AddFrom...` (generated binding/reflection data) | {(program.Contains("ReflectionCache") ? "present" : "absent")}; generator output: {generated.ReflectionData} |");
    sb.AppendLine($"| `InvariantGlobalization` | {(csproj.Contains("<InvariantGlobalization>true") ? "set in WebApi.csproj (ExtraOptimize only)" : "not set")}; not passed by this lab |");
    sb.AppendLine($"| AOT black-box tests (`AppFixture` + `NativeAotTestMode`) | {(Directory.Exists(Path.Join(repoRoot, "tests")) && Directory.EnumerateFiles(Path.Join(repoRoot, "tests"), "*.csproj", SearchOption.AllDirectories).Any(f => File.ReadAllText(f).Contains("NativeAotTestMode")) ? "present" : "absent; integration tests use WebApplicationFactory, which cannot host a native binary")} |");
    sb.AppendLine($"| Dapper.AOT interceptors in Infrastructure | {generated.DapperAot} |");
    sb.AppendLine();
    if (skipGuided) sb.AppendLine("The guided variant was skipped (`--skip-guided`).");
    else
    {
        sb.AppendLine("Variant `native-aot-fe-guidance` publishes a scratch copy of `src/` with these lab-only changes:");
        sb.AppendLine();
        foreach (var patch in guidedPatches) sb.AppendLine($"- {patch}");
        sb.AppendLine("- `-p:JsonSerializerIsReflectionEnabledByDefault=true` on the command line: reflection-based JSON as a stand-in for generated serializer contexts (FastEndpoints generates those with a CLI tool and expects them committed).");
        sb.AppendLine("- Not attempted: an EF Core compiled model / precompiled queries (`dotnet ef dbcontext optimize --nativeaot`), `InvariantGlobalization`, Dapper.AOT opt-in, OpenAPI export at publish time.");
        if (guided is { Binary: not null })
        {
            sb.AppendLine($"\nIts publish produced {guidedWarnings.Count} unique trim/AOT diagnostics (as-is publish: {warnings.Count}). By family: {string.Join(", ", guidedWarnings.GroupBy(w => w.Origin).OrderByDescending(g => g.Count()).Select(g => $"{g.Key} {g.Count()}"))}.");
            var onlyGuided = guidedWarnings.Select(w => w.Origin).Distinct().Except(warnings.Select(w => w.Origin)).ToList();
            if (onlyGuided.Count > 0)
                sb.AppendLine($"Families that appear only in this variant: {string.Join(", ", onlyGuided)}. Likely explanation (an interpretation, not measured): with assembly scanning the endpoint classes are reached only through reflection, so ILC neither analyses nor fully keeps them; generated discovery roots them and their dependencies enter the analysis.");
        }
        else if (guided is not null)
            sb.AppendLine("\nIts publish failed; see the publish errors above.");
    }
    sb.AppendLine();

    sb.AppendLine("## Runtime probes");
    sb.AppendLine();
    sb.AppendLine($"Migration through the native binary: {migrationNote}.");
    sb.AppendLine("The probe database itself was migrated by the JIT lab process, so readiness reflects the host, not the native migrator.");
    sb.AppendLine("Authenticated probes use a lab-minted RS256 `at+jwt` token for a seeded administrator, validated against a stand-in discovery document/JWKS served by the lab.");
    sb.AppendLine();
    if (probes.Count == 0) sb.AppendLine("No binary was produced, so nothing was started.");
    else
    {
        var variants = probes.Values.ToList();
        sb.AppendLine($"| Measure | {string.Join(" | ", variants.Select(v => v.Variant))} |");
        sb.AppendLine($"|---|{string.Concat(variants.Select(_ => "---|"))}");
        sb.AppendLine($"| Start to first `/healthz` 200 | {string.Join(" | ", variants.Select(v => v.Startup is { } s ? $"{s.TotalMilliseconds:F0} ms" : $"did not become healthy (exit {v.ExitCode?.ToString() ?? "still running"})"))} |");
        sb.AppendLine($"| Resident memory after probes | {string.Join(" | ", variants.Select(v => v.ResidentBytes is { } r ? Mb(r) : "-"))} |");
        foreach (var name in variants.SelectMany(v => v.Probes).Select(p => (p.Name, p.Request, p.Expected)).Distinct())
            sb.AppendLine($"| {name.Name}: `{name.Request}` (expect {name.Expected}) | {string.Join(" | ", variants.Select(v => v.Probes.FirstOrDefault(p => p.Name == name.Name) is { } p ? $"{(p.Passed ? "ok" : "**differs**")} {p.Status?.ToString() ?? "no response"}" : "-"))} |");
        sb.AppendLine();
        foreach (var v in variants)
        {
            var failed = v.Probes.Where(p => !p.Passed).ToList();
            if (failed.Count == 0 && v.Startup is not null) continue;
            sb.AppendLine($"### {v.Variant}: unexpected results");
            sb.AppendLine();
            if (v.Startup is null)
                sb.AppendLine($"- The host did not answer `/healthz` with 200 (exit code {v.ExitCode?.ToString() ?? "none, still running after 60 s"}). First error: `{Shorten(FirstError(v.Log), 300).Replace('`', '\'')}`");
            foreach (var p in failed) sb.AppendLine($"- {p.Name}: got {p.Status?.ToString() ?? "no response"}; body `{p.Detail.Replace('`', '\'')}`");
            var exceptions = v.Log.Where(line => line.Contains("Exception", StringComparison.Ordinal)).Select(line => Shorten(line.Trim(), 240)).Distinct().Take(8).ToList();
            if (exceptions.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Exception lines from the host log (first 8 distinct):");
                sb.AppendLine();
                foreach (var line in exceptions) sb.AppendLine($"- `{line.Replace('`', '\'')}`");
                // The first non-BCL frames usually name the library or code path that broke.
                var frames = v.Log.Select(line => line.Trim()).Where(line => line.StartsWith("at ", StringComparison.Ordinal) && !line.StartsWith("at System.", StringComparison.Ordinal))
                    .Distinct().Take(4).ToList();
                if (frames.Count > 0)
                {
                    sb.AppendLine();
                    sb.AppendLine("First non-BCL stack frames:");
                    sb.AppendLine();
                    foreach (var frame in frames) sb.AppendLine($"- `{Shorten(frame, 200).Replace('`', '\'')}`");
                }
            }
            sb.AppendLine();
        }
    }

    sb.AppendLine("## What this does not show");
    sb.AppendLine();
    sb.AppendLine("- It is one machine and one run: startup, memory and size vary with OS, CPU, RID and SDK. Linux container images (the deploy target) were not built.");
    sb.AppendLine("- Probes touch a handful of endpoints. Passing probes do not prove the other endpoints, SSE streams, Delta ETags or telemetry export work natively; failing ones show only the first breaking path.");
    sb.AppendLine("- Few diagnostics do not mean compatible: libraries that use reflection without trim/AOT annotations fail at run time without any ILC warning. Read the diagnostics together with the probes. Zero warnings plus passing behavioural tests would be the minimum before claiming Native AOT support; a binary that starts is not that evidence.");
    sb.AppendLine("- The token comes from a stand-in issuer, not IdentityApi; IdentityApi and GrpcServer were not published natively.");
    return sb.ToString();
}

(string? Project, List<string> Patches) PrepareGuidedCopy()
{
    var root = Path.Join(work, "guided-src");
    var patches = new List<string>();
    File.Copy(Path.Join(repoRoot, "global.json"), Path.Join(root, "global.json").EnsureDirectory());
    File.Copy(Path.Join(repoRoot, "src", "Directory.Packages.props"), Path.Join(root, "src", "Directory.Packages.props").EnsureDirectory());
    foreach (var project in new[] { "Domain", "Application", "Infrastructure", "WebApi" })
    {
        var source = Path.Join(repoRoot, "src", project);
        foreach (var file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(source, file);
            if (relative.Split(Path.DirectorySeparatorChar)[0] is "bin" or "obj") continue;
            File.Copy(file, Path.Join(root, "src", project, relative).EnsureDirectory());
        }
    }

    var programPath = Path.Join(root, "src", "WebApi", "Program.cs");
    var program = File.ReadAllText(programPath);
    if (program.Contains("DiscoveredTypes.All", StringComparison.Ordinal)) patches.Add("`AddFastEndpoints(DiscoveredTypes.All)`: already used by this tree (unchanged).");
    else
    {
        var discovery = System.Text.RegularExpressions.Regex.Match(program, @"\.AddFastEndpoints\(o =>\s*\{[^}]*\}\)");
        if (!discovery.Success) { patches.Add("Could not find the `AddFastEndpoints(...)` call to patch; variant skipped."); return (null, patches); }
        program = program.Replace(discovery.Value, ".AddFastEndpoints(WebApi.DiscoveredTypes.All)");
        patches.Add("`AddFastEndpoints(o => { DisableAutoDiscovery; Assemblies = [...] })` replaced by `AddFastEndpoints(WebApi.DiscoveredTypes.All)` (generated discovery, no assembly scanning).");
    }
    const string singleLine = """app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");""";
    var block = System.Text.RegularExpressions.Regex.Match(program, @"app\.UseFastEndpoints\(c =>\s*\{");
    if (program.Contains(singleLine, StringComparison.Ordinal))
        program = program.Replace(singleLine, """app.UseFastEndpoints(c => { c.Endpoints.RoutePrefix = "api"; WebApi.GeneratedReflection.AddFromWebApi(c.Binding.ReflectionCache); });""");
    else if (block.Success)
        program = program.Replace(block.Value, block.Value + "\n    WebApi.GeneratedReflection.AddFromWebApi(c.Binding.ReflectionCache);");
    else { patches.Add("Could not find the `UseFastEndpoints(...)` call to patch; variant skipped."); return (null, patches); }
    patches.Add("`c.Binding.ReflectionCache.AddFromWebApi()` (the generated `WebApi.GeneratedReflection` extension) added to `UseFastEndpoints` (generated binding/reflection data).");
    File.WriteAllText(programPath, program);
    return (Path.Join(root, "src", "WebApi", "WebApi.csproj"), patches);
}

string FindSwaggerSuppression()
{
    var props = File.ReadAllText(Path.Join(repoRoot, "src", "Directory.Packages.props"));
    var version = System.Text.RegularExpressions.Regex.Match(props, "\"FastEndpoints.Swagger\" Version=\"([^\"]+)\"").Groups[1].Value;
    var packages = Environment.GetEnvironmentVariable("NUGET_PACKAGES") is { Length: > 0 } configured
        ? configured
        : Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
    var targets = Path.Join(packages, "fastendpoints.swagger", version, "build", "FastEndpoints.Swagger.targets");
    if (!File.Exists(targets)) return $"not inspected (`{targets}` not found)";
    var text = File.ReadAllText(targets);
    return text.Contains("<SuppressTrimAnalysisWarnings>true", StringComparison.Ordinal) && text.Contains("<SuppressAotAnalysisWarnings>true", StringComparison.Ordinal)
        ? $"version {version} sets `SuppressTrimAnalysisWarnings` and `SuppressAotAnalysisWarnings` to true unconditionally"
        : $"version {version} does not set the suppression properties";
}

static string FirstError(List<string> log) =>
    (log.FirstOrDefault(line => line.Contains("Unhandled exception", StringComparison.Ordinal))
     ?? log.FirstOrDefault(line => line.Contains("Exception", StringComparison.Ordinal))
     ?? Tail(log, 1)).Trim();

GeneratedInfo InspectGeneratedSources(string artifacts)
{
    string discovered = "not found", reflection = "not found", dapper = "not found";
    var objRoot = Path.Join(artifacts, "obj");
    if (!Directory.Exists(objRoot)) return new(discovered, reflection, dapper);
    var files = Directory.EnumerateFiles(objRoot, "*.cs", SearchOption.AllDirectories)
        .Where(f => f.Contains($"{Path.DirectorySeparatorChar}generated{Path.DirectorySeparatorChar}", StringComparison.Ordinal)).ToList();
    var webApi = files.Where(f => f.Contains($"{Path.DirectorySeparatorChar}WebApi{Path.DirectorySeparatorChar}", StringComparison.Ordinal)).ToList();
    if (webApi.FirstOrDefault(f => File.ReadAllText(f).Contains("class DiscoveredTypes")) is { } d)
        discovered = $"generated (`{Path.GetFileName(d)}`, {File.ReadAllText(d).Split("Preserve<").Length - 1} types)";
    if (webApi.FirstOrDefault(f => File.ReadAllText(f).Contains("ReflectionCache", StringComparison.Ordinal) || Path.GetFileName(f).Contains("Reflection", StringComparison.OrdinalIgnoreCase)) is { } r)
        reflection = $"generated (`{Path.GetFileName(r)}`)";
    var interceptors = files.Where(f => f.Contains($"{Path.DirectorySeparatorChar}Infrastructure{Path.DirectorySeparatorChar}", StringComparison.Ordinal) && f.Contains("Dapper.AOT", StringComparison.Ordinal))
        .Sum(f => File.ReadAllText(f).Split("InterceptsLocation(").Length - 1);
    dapper = interceptors > 0 ? $"{interceptors} generated call-site interceptors" : "none generated (Dapper calls stay reflection/emit based)";
    return new(discovered, reflection, dapper);
}

async Task<(int? Exit, List<string> Output, TimeSpan Elapsed)> RunToExitAsync(string file, string[] arguments, string directory,
    Dictionary<string, string> environment, TimeSpan timeout, string? echo = null)
{
    var start = new ProcessStartInfo(file) { WorkingDirectory = directory, RedirectStandardOutput = true, RedirectStandardError = true };
    foreach (var argument in arguments) start.ArgumentList.Add(argument);
    // `dotnet run` can leak MSBuild variables into its child; a nested publish must not inherit them.
    foreach (var key in start.Environment.Keys.Where(k => k.StartsWith("MSBuild", StringComparison.OrdinalIgnoreCase)).ToList()) start.Environment.Remove(key);
    foreach (var (key, value) in environment) start.Environment[key] = value;
    var output = new List<string>();
    var watch = Stopwatch.StartNew();
    using var process = new Process { StartInfo = start };
    void Collect(string? line)
    {
        if (line is null) return;
        lock (output) output.Add(line);
        if (echo is not null && (line.Contains(" -> ", StringComparison.Ordinal) || line.Contains("Generating native code", StringComparison.Ordinal) || line.Contains(": error ", StringComparison.Ordinal)))
            Console.WriteLine($"[{echo}] {line.Trim()}");
    }
    process.OutputDataReceived += (_, e) => Collect(e.Data);
    process.ErrorDataReceived += (_, e) => Collect(e.Data);
    try { process.Start(); }
    catch (System.ComponentModel.Win32Exception e) { return (null, [$"could not start {file}: {e.Message}"], watch.Elapsed); }
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    using var cancel = new CancellationTokenSource(timeout);
    try { await process.WaitForExitAsync(cancel.Token); }
    catch (OperationCanceledException)
    {
        process.Kill(entireProcessTree: true);
        await process.WaitForExitAsync();
        lock (output) return (null, [.. output], watch.Elapsed);
    }
    lock (output) return (process.ExitCode, [.. output], watch.Elapsed);
}

async Task<string?> TryRunAsync(string file, string[] arguments, string directory)
{
    var (exit, output, _) = await RunToExitAsync(file, arguments, directory, [], TimeSpan.FromMinutes(1));
    return exit == 0 ? string.Join('\n', output).Trim() : null;
}

static async Task ServeDiscoveryAsync(HttpListener listener, string issuer, string jwksUri, RSA rsa, string keyId, CancellationToken cancellationToken)
{
    var parameters = rsa.ExportParameters(false);
    var configuration = JsonSerializer.Serialize(new Dictionary<string, object>
    {
        ["issuer"] = issuer,
        ["jwks_uri"] = jwksUri,
        ["id_token_signing_alg_values_supported"] = new[] { "RS256" }
    });
    var keys = JsonSerializer.Serialize(new { keys = new[] { new { kty = "RSA", use = "sig", alg = "RS256", kid = keyId, n = Base64Url(parameters.Modulus!), e = Base64Url(parameters.Exponent!) } } });
    while (!cancellationToken.IsCancellationRequested)
    {
        var context = await listener.GetContextAsync().WaitAsync(cancellationToken);
        var body = Encoding.UTF8.GetBytes(context.Request.Url?.AbsolutePath == "/jwks" ? keys : configuration);
        context.Response.ContentType = "application/json";
        context.Response.ContentLength64 = body.Length;
        await context.Response.OutputStream.WriteAsync(body, cancellationToken);
        context.Response.Close();
    }
}

static string MintToken(RSA rsa, string keyId, string issuer, string audience, Guid userId, string login, string securityStamp)
{
    var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    var header = JsonSerializer.Serialize(new Dictionary<string, object> { ["alg"] = "RS256", ["typ"] = "at+jwt", ["kid"] = keyId });
    var payload = JsonSerializer.Serialize(new Dictionary<string, object>
    {
        ["iss"] = issuer, ["aud"] = audience, ["sub"] = userId.ToString(),
        [CpnucleoClaimTypes.Login] = login, [CpnucleoClaimTypes.SecurityStamp] = securityStamp, [CpnucleoClaimTypes.Admin] = "true",
        ["iat"] = now, ["nbf"] = now - 5, ["exp"] = now + 1800
    });
    var unsigned = $"{Base64Url(Encoding.UTF8.GetBytes(header))}.{Base64Url(Encoding.UTF8.GetBytes(payload))}";
    var signature = rsa.SignData(Encoding.ASCII.GetBytes(unsigned), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    return $"{unsigned}.{Base64Url(signature)}";
}

static string Base64Url(byte[] value) => Convert.ToBase64String(value).TrimEnd('=').Replace('+', '-').Replace('/', '_');

static int FreePort()
{
    using var listener = new TcpListener(IPAddress.Loopback, 0);
    listener.Start();
    return ((IPEndPoint)listener.LocalEndpoint).Port;
}

static long FolderSize(string path) => Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
    .Where(f => !f.Contains(".dSYM", StringComparison.Ordinal) && !f.EndsWith(".dbg", StringComparison.Ordinal) && !f.EndsWith(".pdb", StringComparison.Ordinal))
    .Sum(f => new FileInfo(f).Length);

static string Mb(long bytes) => $"{bytes / 1024d / 1024d:F1} MB";

static string Shorten(string value, int max = 200)
{
    var single = value.ReplaceLineEndings(" ");
    return single.Length <= max ? single : single[..max] + "...";
}

static string Tail(List<string> lines, int count) => string.Join(" / ", lines.Where(l => !string.IsNullOrWhiteSpace(l)).TakeLast(count).Select(l => Shorten(l.Trim(), 300)));

static string RealPath(string path)
{
    var full = Path.GetFullPath(path);
    var current = Path.GetPathRoot(full)!;
    foreach (var segment in full[current.Length..].Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries))
    {
        current = Path.Join(current, segment);
        if (new DirectoryInfo(current).ResolveLinkTarget(returnFinalTarget: true) is { } target) current = target.FullName;
    }
    return current;
}

static string ResolveDotnet()
{
    if (Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") is { Length: > 0 } host && File.Exists(host)) return host;
    var name = OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet";
    foreach (var directory in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(Path.PathSeparator))
        if (directory.Length > 0 && File.Exists(Path.Join(directory, name))) return Path.Join(directory, name);
    return name;
}

static string FindRepoRoot()
{
    foreach (var start in new[] { Environment.CurrentDirectory, AppContext.BaseDirectory })
        for (var directory = new DirectoryInfo(start); directory is not null; directory = directory.Parent)
            if (File.Exists(Path.Join(directory.FullName, "cpnucleo.slnx"))) return directory.FullName;
    throw new InvalidOperationException("Run the lab from inside the Cpnucleo repository.");
}

sealed record PublishResult(string Variant, string Command, int? ExitCode, TimeSpan Duration, List<string> Output, string LogPath,
    string PublishDir, string? Binary, bool ToolingFailed);

sealed record ProbeResult(string Name, string Request, int Expected, int? Status, bool Passed, TimeSpan Elapsed, string Detail);

sealed record RunResult(string Variant, TimeSpan? Startup, long? ResidentBytes, int? ExitCode, List<ProbeResult> Probes, List<string> Log);

sealed record GeneratedInfo(string DiscoveredTypes, string ReflectionData, string DapperAot);

static class PathExtensions
{
    /// <summary>Creates the parent directory of a file path and returns the path.</summary>
    public static string EnsureDirectory(this string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        return path;
    }
}
