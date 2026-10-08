using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OpenApiLab;

/// <summary>
/// Generates WebApi's OpenAPI document with FastEndpoints.OpenApi (Microsoft.AspNetCore.OpenApi) for the
/// real endpoint classes and compares it with the committed FastEndpoints.Swagger (NSwag) document.
/// Claimed property (non-zero exit when it fails): both generators describe the same operations.
/// </summary>
internal static class Lab
{
    private const string OutOfTheBoxFlag = "--out-of-the-box";

    private static async Task<int> Main(string[] args)
    {
        if (args.Contains(OutOfTheBoxFlag))
        {
            // Child process. FastEndpoints applies some settings once per process: after an unpatched host had run,
            // the workaround no longer took effect in a second host of the same process, so the attempts are isolated.
            var attempt = (await LabHost.GenerateAsync(alignDefaultValues: false, LabHost.Equivalent))[0];
            Console.WriteLine(attempt.StatusCode == 200 ? "HTTP 200" : $"HTTP {attempt.StatusCode}: {attempt.Body.Split('\n')[0].Trim()}");
            return 0;
        }

        var stopwatch = Stopwatch.StartNew();
        var snapshot = Path.Join(RepositoryRoot(), "docs", "openapi", "webapi.v1.json");
        var nswag = JsonNode.Parse(await File.ReadAllTextAsync(snapshot))!.AsObject();
        Console.WriteLine($"Reference: docs/openapi/webapi.v1.json (FastEndpoints.Swagger/NSwag, committed snapshot)");
        Console.WriteLine($"Candidate: FastEndpoints.OpenApi {Version(typeof(FastEndpoints.OpenApi.DocumentOptions))} on Microsoft.AspNetCore.OpenApi " +
            $"{Version(typeof(Microsoft.AspNetCore.OpenApi.OpenApiOptions))}, endpoints from WebApi.DiscoveredTypes.All, in-process loopback host");
        Console.WriteLine();

        // 1. Out of the box: WebApi's DTOs exactly as they are.
        var raw = await RunOutOfTheBoxAsync();
        Console.WriteLine("[1] Out of the box, equivalent settings, no workaround (separate process)");
        if (raw == "HTTP 200")
            Console.WriteLine("  Generation    HTTP 200 (the [DefaultValue] incompatibility no longer reproduces; the workaround below is unnecessary)");
        else
        {
            Console.WriteLine($"  Generation    {raw}");
            Console.WriteLine("                  cause: WebApi DTOs carry [DefaultValue(\"<uuid>\")] / [DefaultValue(\"2064-06-09\")] strings on Guid/DateTime properties;");
            Console.WriteLine("                  NSwag copies them, Microsoft.AspNetCore.OpenApi serializes them with the property's own type.");
        }
        Console.WriteLine();

        // 2. Same host with a schema-metadata-only workaround, two documents: settings only, and settings plus transformers.
        var documents = await LabHost.GenerateAsync(alignDefaultValues: true, LabHost.Equivalent, LabHost.Transformed);
        var output = Path.Join(AppContext.BaseDirectory, "openapi");
        Directory.CreateDirectory(output);
        var failures = 0;
        foreach (var document in documents)
        {
            Console.WriteLine(document.Name == LabHost.Equivalent
                ? "[2] FastEndpoints.OpenApi with WebApi's equivalent settings (DefaultValue workaround applied)"
                : "[3] Same plus the two NSwag processors ported as operation transformers and NSwag-style schema ids (differences left = remaining gaps)");
            if (document.StatusCode != 200)
            {
                Console.WriteLine($"  Generation    FAILED: HTTP {document.StatusCode}: {document.Body.Split('\n')[0].Trim()}");
                failures++;
                continue;
            }
            var generated = JsonNode.Parse(document.Body)!.AsObject();
            var file = Path.Join(output, $"fastendpoints-openapi.{document.Name}.json");
            await File.WriteAllTextAsync(file, generated.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) + "\n");
            Console.WriteLine($"  Written       {Path.GetRelativePath(RepositoryRoot(), file)}");
            if (!new Comparison(nswag, generated).Report()) failures++;
            Console.WriteLine();
        }

        Console.WriteLine(string.Create(CultureInfo.InvariantCulture,
            $"Runtime: {stopwatch.Elapsed.TotalSeconds:0.0} s (one child process for [1], one in-process host for [2] and [3]; no database, no Docker)"));
        if (failures > 0)
        {
            Console.WriteLine("FAILED: FastEndpoints.OpenApi did not describe the same operations (path + verb) as the committed NSwag document.");
            return 1;
        }
        Console.WriteLine("Verified: FastEndpoints.OpenApi describes the same 'path + verb' operations as the committed NSwag document, " +
            "with and without the ported transformers. Every other difference above is reported, not asserted.");
        return 0;
    }

    private static async Task<string> RunOutOfTheBoxAsync()
    {
        // Under `dotnet OpenApiLab.dll` the process is the dotnet host; under the apphost it is the lab itself.
        var host = Environment.ProcessPath!;
        var start = new ProcessStartInfo(host) { RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false };
        if (Path.GetFileNameWithoutExtension(host).Equals("dotnet", StringComparison.OrdinalIgnoreCase))
            start.ArgumentList.Add(typeof(Lab).Assembly.Location);
        start.ArgumentList.Add(OutOfTheBoxFlag);
        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        var line = (await output).Trim();
        return process.ExitCode == 0 && line.Length > 0 ? line : $"child process failed (exit {process.ExitCode}): {(await error).Trim()}";
    }

    private static string Version(Type type) =>
        type.Assembly.GetCustomAttributes(typeof(System.Reflection.AssemblyInformationalVersionAttribute), false)
            is [System.Reflection.AssemblyInformationalVersionAttribute { InformationalVersion: var version }]
            ? version.Split('+')[0]
            : type.Assembly.GetName().Version?.ToString() ?? "?";

    private static string RepositoryRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
            if (File.Exists(Path.Join(directory.FullName, "cpnucleo.slnx"))) return directory.FullName;
        throw new InvalidOperationException("Run the lab from the repository (cpnucleo.slnx not found above the build output).");
    }
}
