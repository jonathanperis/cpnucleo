using System.Buffers.Text;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Scalar.AspNetCore;

// Scalar API reference served beside the committed OpenAPI contracts (docs/openapi), under the
// security headers WebApi sends. Two profiles are compared:
//   /scalar-defaults/{doc}: Scalar's defaults under WebApi's exact /swagger CSP.
//   /scalar/{doc}:          no CDN fonts, telemetry, agent, MCP or hosted-client link; scripts allowed only by a per-request nonce.
// Styles keep 'unsafe-inline': in Chrome, Scalar adds <style> elements without the nonce and sets style attributes,
// and any nonce or hash in style-src makes browsers ignore 'unsafe-inline'.
// `--serve` keeps the host running so a real browser can be pointed at it; the lab itself needs neither.

const string SwaggerCsp = "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; frame-ancestors 'none'";
const string ApiCsp = "default-src 'none'; frame-ancestors 'none'; base-uri 'none'";
const string NonceKey = "lab-csp-nonce";
static string HardenedCsp(string nonce) =>
    $"default-src 'none'; script-src 'nonce-{nonce}'; style-src 'unsafe-inline'; img-src 'self'; connect-src 'self'; " +
    "frame-ancestors 'none'; base-uri 'none'; form-action 'none'";

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
var stopwatch = Stopwatch.StartNew();
var serve = args.Contains("--serve");
var repository = FindRepository();
string[] documents = ["webapi", "identityapi"];

var builder = WebApplication.CreateSlimBuilder();
builder.WebHost.UseUrls("http://127.0.0.1:0");
builder.Logging.ClearProviders();
var app = builder.Build();

// Same headers as src/WebApi/Program.cs, with the CSP chosen per lab profile.
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    if (path.StartsWithSegments("/scalar")) context.Items[NonceKey] = Base64Url.EncodeToString(RandomNumberGenerator.GetBytes(32));
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.TryAdd("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        context.Response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
        context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
        context.Response.Headers.TryAdd("Referrer-Policy", "strict-origin-when-cross-origin");
        var csp = path.StartsWithSegments("/scalar-defaults") ? SwaggerCsp
            : path.StartsWithSegments("/scalar") ? HardenedCsp((string)context.Items[NonceKey]!)
            : ApiCsp;
        context.Response.Headers.TryAdd("Content-Security-Policy", csp);
        return Task.CompletedTask;
    });
    await next();
});

// The committed contracts, read from the repository on every request.
app.MapGet("/openapi/{name}.json", (string name) => documents.Contains(name)
    ? Results.File(Path.Join(repository, "docs", "openapi", $"{name}.v1.json"), "application/json")
    : Results.NotFound());

app.MapScalarApiReference("/scalar-defaults", options => options.AddDocuments(documents));
app.MapScalarApiReference("/scalar", (options, context) =>
{
    var nonce = (string)context.Items[NonceKey]!;
    options.AddDocuments(documents)
        .DisableDefaultFonts()
        .DisableTelemetry()
        .DisableAgent()
        .DisableMcp()
        // The sidebar "Open API Client" link otherwise opens client.scalar.com with this document's URL;
        // navigation is outside CSP's reach.
        .HideClientButton()
        .WithNonce(nonce);
});

await app.StartAsync();
var origin = new Uri(app.Urls.First());
using var http = new HttpClient { BaseAddress = origin };
var failures = new List<string>();
void Require(bool condition, string failure) { if (!condition) failures.Add(failure); }

var scalarVersion = typeof(ScalarOptions).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0];
Console.WriteLine($"Scalar.AspNetCore {scalarVersion}; host {origin}; contracts from {Path.Join(repository, "docs", "openapi")}");

// 1. Documents: byte-identical to the committed files and recognisable OpenAPI 3 documents.
foreach (var name in documents)
{
    using var response = await http.GetAsync($"/openapi/{name}.json");
    var bytes = await response.Content.ReadAsByteArrayAsync();
    var committed = await File.ReadAllBytesAsync(Path.Join(repository, "docs", "openapi", $"{name}.v1.json"));
    using var json = JsonDocument.Parse(bytes);
    var title = json.RootElement.GetProperty("info").GetProperty("title").GetString();
    var paths = json.RootElement.GetProperty("paths").EnumerateObject().Count();
    Require(response.IsSuccessStatusCode && bytes.AsSpan().SequenceEqual(committed) && json.RootElement.GetProperty("openapi").GetString()!.StartsWith('3'),
        $"/openapi/{name}.json is not the committed OpenAPI document");
    Console.WriteLine($"Document /openapi/{name}.json: {(int)response.StatusCode}, {bytes.Length} bytes, identical to the committed file, \"{title}\", {paths} paths");
}

// 2. Reference pages and every asset they reference, per profile.
string? bundle = null;
foreach (var (prefix, label) in new[] { ("/scalar-defaults", "Scalar defaults under WebApi's /swagger CSP"), ("/scalar", "hardened options under a nonce CSP") })
{
    Console.WriteLine();
    Console.WriteLine($"Profile {prefix}: {label}");
    var nonces = new HashSet<string>();
    foreach (var name in documents)
    {
        var pageUri = new Uri(origin, $"{prefix}/{name}");
        using var page = await http.GetAsync(pageUri);
        var html = await page.Content.ReadAsStringAsync();
        var csp = Csp.Parse(page.Headers.TryGetValues("Content-Security-Policy", out var values) ? values.Single() : "");
        Require(page.IsSuccessStatusCode && page.Content.Headers.ContentType?.MediaType == "text/html", $"{pageUri.AbsolutePath} was not served as HTML");
        Require(csp.Directives.Count > 0, $"{pageUri.AbsolutePath} has no CSP header");
        Require(page.Headers.Contains("X-Content-Type-Options") && page.Headers.Contains("X-Frame-Options"), $"{pageUri.AbsolutePath} lacks WebApi's security headers");

        var scripts = Regex.Matches(html, @"<script\b([^>]*)>(.*?)</script>", RegexOptions.Singleline)
            .Select(m => (Src: Attribute(m.Groups[1].Value, "src"), Nonce: Attribute(m.Groups[1].Value, "nonce"), Inline: m.Groups[2].Value.Trim())).ToList();
        var configJson = Regex.Match(html, @"initialize\(\s*'([^']*)',\s*(true|false),\s*(\{.*\}),\s*'[^']*'\)", RegexOptions.Singleline);
        Require(configJson.Success, $"{pageUri.AbsolutePath}: Scalar configuration not found in the page");
        if (!configJson.Success) continue; // Recorded as a failure; the remaining pages and profiles are still checked.
        using var config = JsonDocument.Parse(configJson.Groups[3].Value);
        var initPath = Uri.UnescapeDataString(configJson.Groups[1].Value);
        // scalar.aspnetcore.js resolves relative sources against origin + (page path minus the initialize path) + "/".
        var basePath = pageUri.AbsolutePath.EndsWith(initPath, StringComparison.Ordinal) ? pageUri.AbsolutePath[..^initPath.Length] : "";
        var sourceBase = new Uri(origin, basePath + "/");

        var assets = new List<(string Kind, Uri Url, string Directive, string? Nonce)>();
        foreach (var script in scripts.Where(s => s.Src is not null)) assets.Add(("script", new Uri(pageUri, script.Src!), "script-src", script.Nonce));
        foreach (Match link in Regex.Matches(html, @"<link\b[^>]*\bhref=""([^""]+)""")) assets.Add(("link", new Uri(pageUri, link.Groups[1].Value), "style-src", null));
        if (config.RootElement.TryGetProperty("favicon", out var favicon)) assets.Add(("favicon", new Uri(pageUri, favicon.GetString()!), "img-src", null));
        foreach (var source in config.RootElement.GetProperty("sources").EnumerateArray())
            assets.Add(("document (fetch)", new Uri(sourceBase, source.GetProperty("url").GetString()!), "connect-src", null));

        foreach (var asset in assets)
        {
            using var response = await http.GetAsync(asset.Url);
            var body = await response.Content.ReadAsByteArrayAsync();
            var sameOrigin = asset.Url.GetLeftPart(UriPartial.Authority) == origin.GetLeftPart(UriPartial.Authority);
            var allowed = csp.AllowsUrl(asset.Directive, asset.Url, origin, asset.Nonce);
            Require(sameOrigin, $"{pageUri.AbsolutePath}: {asset.Url} is not same-origin");
            Require(response.StatusCode == System.Net.HttpStatusCode.OK, $"{pageUri.AbsolutePath}: {asset.Url.AbsolutePath} returned {(int)response.StatusCode}");
            Require(allowed, $"{pageUri.AbsolutePath}: {asset.Url.AbsolutePath} is not allowed by {asset.Directive}");
            if (name == documents[0])
                Console.WriteLine($"  {asset.Kind,-16} {asset.Url.AbsolutePath,-38} {(int)response.StatusCode} {response.Content.Headers.ContentType?.MediaType,-22} {body.Length,9} bytes  same-origin={sameOrigin}  {asset.Directive} allows={allowed}");
            if (asset.Url.AbsolutePath.EndsWith("/scalar.js", StringComparison.Ordinal)) bundle ??= System.Text.Encoding.UTF8.GetString(body);
        }

        var inline = scripts.Where(s => s.Src is null).ToList();
        foreach (var script in inline) Require(csp.AllowsInlineScript(script.Nonce), $"{pageUri.AbsolutePath}: inline script blocked by script-src");
        if (prefix == "/scalar")
        {
            var headerNonce = csp.Nonce("script-src");
            Require(!csp.Has("script-src", "'unsafe-inline'") && headerNonce is not null, "hardened script-src must rely on a nonce, not 'unsafe-inline'");
            Require(scripts.All(s => s.Nonce == headerNonce), $"{pageUri.AbsolutePath}: not every script tag carries the header nonce");
            Require(csp.Has("style-src", "'unsafe-inline'") && !csp.Directives["style-src"].Any(s => s.StartsWith("'nonce-", StringComparison.Ordinal) || s.StartsWith("'sha", StringComparison.Ordinal)),
                "hardened style-src must keep 'unsafe-inline' without nonces or hashes (Scalar's runtime styles carry no nonce)");
            nonces.Add(headerNonce ?? "");
        }
        if (name == documents[0])
        {
            Console.WriteLine($"  inline scripts   {inline.Count} (allowed via {(csp.Nonce("script-src") is null ? "'unsafe-inline'" : "nonce")})");
            Console.WriteLine($"  config           {string.Join(", ", config.RootElement.EnumerateObject().Where(p => p.Name is not "sources" and not "favicon").Select(p => $"{p.Name}={p.Value.GetRawText()}"))}");
            Console.WriteLine($"  CSP              {csp.Raw}");
        }
    }
    if (prefix == "/scalar")
    {
        using var again = await http.GetAsync("/scalar/webapi");
        nonces.Add(Csp.Parse(again.Headers.GetValues("Content-Security-Policy").Single()).Nonce("script-src") ?? "");
        Require(nonces.Count == documents.Length + 1, "nonces repeat across requests");
        Console.WriteLine($"  nonce            fresh per response ({nonces.Count} responses, {nonces.Count} distinct nonces)");
    }
}

// 3. What the browser would still do at runtime: static evidence from the served bundle (reported, not asserted).
Console.WriteLine();
Console.WriteLine($"Bundle scan of scalar.js ({bundle?.Length ?? 0:N0} chars), static evidence only:");
if (bundle is not null)
{
    int Count(string pattern) => Regex.Matches(bundle, pattern).Count;
    var hosts = Regex.Matches(bundle, @"https://[a-z0-9.-]*scalar\.com").Select(m => m.Value).Distinct().Order();
    Console.WriteLine($"  scalar.com URLs embedded: {string.Join(", ", hosts)}");
    Console.WriteLine($"  @font-face rules on fonts.scalar.com: {Count(@"url\(https://fonts\.scalar\.com/")} (emitted only when withDefaultFonts is true; the defaults profile does not set it, so the bundle default true applies)");
    Console.WriteLine($"  eval( / new Function(: {Count(@"(?<![\w.$])eval\(") + Count(@"new Function\(")}; try {{ Function(``) }} capability probes: {Count(@"try\{return Function\(``\)")} (when blocked it reports one script-src violation and falls back; no 'unsafe-eval' needed per a local Chrome run, not this lab)");
    Console.WriteLine($"  new Worker / SharedWorker / importScripts: {Count(@"new (Shared)?Worker\(") + Count("importScripts")}  -> worker-src {(Count(@"new (Shared)?Worker\(") + Count("importScripts") == 0 ? "not indicated" : "may be needed")}");
    Console.WriteLine($"  main stylesheet injected at runtime as <style> (nonce read from meta[property=csp-nonce]): {(bundle.Contains("meta[property=csp-nonce]") ? "yes" : "no")}; a local Chrome run showed further un-nonced <style> elements and style attributes -> style-src 'unsafe-inline'");
}

stopwatch.Stop();
Console.WriteLine();
if (failures.Count > 0)
{
    foreach (var failure in failures) Console.Error.WriteLine($"FAILED: {failure}");
    Environment.ExitCode = 1;
}
else
{
    Console.WriteLine("Verified: both committed documents served unchanged; Scalar reference pages served for both; every script, favicon and document they reference is same-origin, 200, and allowed by the response CSP (WebApi's /swagger CSP for defaults, a nonce-only script-src for the hardened profile).");
    Console.WriteLine("Not verified here: rendering, runtime requests, style/font/connect violations. Those need a real browser (see --serve).");
}
Console.WriteLine($"Completed in {stopwatch.Elapsed.TotalSeconds:F1}s.");

if (serve && failures.Count == 0)
{
    Console.WriteLine($"Serving {new Uri(origin, "/scalar/webapi")} and {new Uri(origin, "/scalar-defaults/webapi")}; Ctrl+C to stop.");
    await app.WaitForShutdownAsync();
}
await app.StopAsync();

static string? Attribute(string attributes, string name)
{
    var match = Regex.Match(attributes, $@"\b{name}=""([^""]*)""");
    return match.Success ? match.Groups[1].Value : null;
}

static string FindRepository()
{
    foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        for (var directory = new DirectoryInfo(start); directory is not null; directory = directory.Parent)
            if (File.Exists(Path.Join(directory.FullName, "cpnucleo.slnx")) && Directory.Exists(Path.Join(directory.FullName, "docs", "openapi")))
                return directory.FullName;
    throw new InvalidOperationException("Run from inside the Cpnucleo repository: docs/openapi was not found.");
}

// A deliberately small CSP evaluator: 'self', 'none', nonces, 'unsafe-inline', schemes and exact origins.
sealed record Csp(string Raw, Dictionary<string, string[]> Directives)
{
    public static Csp Parse(string header) => new(header, header.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(d => d.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        .ToDictionary(parts => parts[0].ToLowerInvariant(), parts => parts[1..]));

    string[] Effective(string directive) =>
        Directives.TryGetValue(directive, out var sources) || Directives.TryGetValue("default-src", out sources) ? sources : ["*"];

    public bool Has(string directive, string source) => Effective(directive).Contains(source);

    public string? Nonce(string directive) =>
        Effective(directive).FirstOrDefault(s => s.StartsWith("'nonce-", StringComparison.Ordinal))?[7..^1];

    public bool AllowsUrl(string directive, Uri url, Uri origin, string? nonce)
    {
        var sources = Effective(directive);
        if (nonce is not null && Nonce(directive) == nonce) return true;
        return sources.Any(source => source switch
        {
            "'self'" => url.GetLeftPart(UriPartial.Authority) == origin.GetLeftPart(UriPartial.Authority),
            "*" => url.Scheme is "http" or "https",
            _ when source.EndsWith(':') => url.Scheme + ":" == source,
            _ when source.StartsWith("http", StringComparison.Ordinal) => url.GetLeftPart(UriPartial.Authority) == source.TrimEnd('/'),
            _ => false,
        });
    }

    public bool AllowsInlineScript(string? nonce)
    {
        var sources = Effective("script-src");
        var declared = Nonce("script-src");
        if (declared is not null) return nonce == declared; // a nonce disables 'unsafe-inline'
        return sources.Contains("'unsafe-inline'") && !sources.Any(s => s.StartsWith("'sha", StringComparison.Ordinal));
    }
}
