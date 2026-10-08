using System.Text.RegularExpressions;

/// <summary>One unique trim (IL2xxx) or AOT (IL3xxx) diagnostic and where it was raised.</summary>
sealed record IlWarning(string Code, string Origin, string Member, string Message, bool IsError);

/// <summary>
/// Parses MSBuild/ILC output. Two shapes exist: Roslyn analyzer warnings on project sources
/// (<c>path/File.cs(1,2): warning IL2026: ... [x.csproj]</c>) and ILC whole-program warnings
/// (<c>ILC : Trim analysis warning IL2026: Namespace.Type.Member(...): Using member ...</c>).
/// The origin is the member <em>raising</em> the warning (the caller), not the API it calls.
/// </summary>
static partial class WarningSummary
{
    [GeneratedRegex(@"^(?<location>.*?)\s*:\s*(?:(?:Trim|AOT) analysis\s+)?(?<severity>warning|error)\s+(?<code>IL[23]\d{3})\s*:\s*(?<message>.*?)(?:\s+\[[^\]]+\])?\s*$")]
    private static partial Regex Diagnostic();

    [GeneratedRegex(@"Assembly '(?<assembly>[^']+)'")]
    private static partial Regex AssemblyName();

    [GeneratedRegex(@"[/\\]src[/\\](?<project>Application|Domain|Infrastructure|WebApi)[/\\]")]
    private static partial Regex OwnSource();

    // Longest prefix first; values are the package/assembly family reported to the learner.
    private static readonly (string Prefix, string Origin)[] Families =
    [
        ("Npgsql.EntityFrameworkCore", "Npgsql EF Core provider"),
        ("Microsoft.EntityFrameworkCore", "EF Core"),
        ("Npgsql", "Npgsql"),
        ("NSwag", "NSwag / NJsonSchema"),
        ("NJsonSchema", "NSwag / NJsonSchema"),
        ("Namotion", "NSwag / NJsonSchema"),
        ("Newtonsoft", "Newtonsoft.Json (via NSwag)"),
        ("Delta", "Delta"),
        ("System.Linq.Dynamic.Core", "System.Linq.Dynamic.Core"),
        ("FastEndpoints", "FastEndpoints"),
        ("FluentValidation", "FluentValidation (via FastEndpoints)"),
        ("Dapper", "Dapper"),
        ("OpenTelemetry", "OpenTelemetry"),
        ("Microsoft.AspNetCore", "ASP.NET Core"),
        ("Microsoft.Extensions", "Microsoft.Extensions"),
        ("Microsoft.IdentityModel", "IdentityModel (JWT)"),
        ("System.IdentityModel", "IdentityModel (JWT)"),
        ("Konscious", "Konscious Argon2"),
        ("Bogus", "Bogus"),
        ("Riok.Mapperly", "Mapperly"),
        ("WebApi", "Cpnucleo WebApi (own code)"),
        ("Program", "Cpnucleo WebApi (own code)"),
        ("Application", "Cpnucleo Application (own code)"),
        ("Infrastructure", "Cpnucleo Infrastructure (own code)"),
        ("Domain", "Cpnucleo Domain (own code)"),
        ("System", "BCL (System.*)"),
        ("Microsoft", "Microsoft (other)")
    ];

    public static List<IlWarning> Parse(IEnumerable<string> lines)
    {
        var unique = new Dictionary<string, IlWarning>(StringComparer.Ordinal);
        foreach (var match in lines.Select(line => Diagnostic().Match(line)).Where(match => match.Success))
        {
            var code = match.Groups["code"].Value;
            var location = match.Groups["location"].Value.Trim();
            var message = match.Groups["message"].Value.Trim();
            var member = "";
            string origin;
            if (AssemblyName().Match(message) is { Success: true } assembly && code is "IL2104" or "IL3053")
            {
                origin = Classify(assembly.Groups["assembly"].Value);
                member = assembly.Groups["assembly"].Value;
            }
            else if (OwnSource().Match(location) is { Success: true } own)
            {
                // Roslyn analyzer or ILC warning located in this repository's sources.
                origin = $"Cpnucleo {own.Groups["project"].Value} (own code)";
                member = location[(location.LastIndexOfAny(['/', '\\']) + 1)..];
            }
            else
            {
                // ILC: "Namespace.Type.Member(args): Using member ..."; the member text may contain ": " in generics rarely.
                var separator = message.IndexOf(": ", StringComparison.Ordinal);
                member = separator > 0 ? message[..separator] : "";
                origin = Classify(member.TrimStart('<', '$'));
            }
            var key = $"{code}|{location}|{message}";
            unique.TryAdd(key, new IlWarning(code, origin, member, message, match.Groups["severity"].Value == "error"));
        }
        return [.. unique.Values];
    }

    public static string Classify(string name)
    {
        foreach (var (prefix, origin) in Families)
            if (name.StartsWith(prefix, StringComparison.Ordinal) &&
                (name.Length == prefix.Length || name[prefix.Length] is '.' or '<' or '`' or '(' or '+'))
                return origin;
        var first = name.Split('.', '<', '(', '`')[0];
        return string.IsNullOrWhiteSpace(first) ? "(unattributed)" : first;
    }

    public static string Meaning(string code) => code switch
    {
        "IL2026" => "calls a member marked RequiresUnreferencedCode (reflection the trimmer cannot see)",
        "IL2055" or "IL2060" => "MakeGenericType/MakeGenericMethod with types unknown at compile time",
        "IL2057" => "Type.GetType with a non-constant string",
        "IL2062" or "IL2067" or "IL2068" or "IL2069" or "IL2070" or "IL2072" or "IL2075" or "IL2077" or "IL2080" or "IL2087" or "IL2090" or "IL2091"
            => "DynamicallyAccessedMembers requirement not satisfied (reflection over a type the trimmer cannot track)",
        "IL2104" => "assembly produced trim warnings (collapsed; run without --single-warn for detail)",
        "IL2111" => "method with DynamicallyAccessedMembers accessed via reflection",
        "IL3000" => "Assembly.Location is empty in single-file/native output",
        "IL3050" => "calls a member marked RequiresDynamicCode (runtime code generation, unavailable in Native AOT)",
        "IL3051" or "IL3052" => "RequiresDynamicCode attribute mismatch/annotation issue",
        "IL3053" => "assembly produced AOT analysis warnings (collapsed)",
        _ when code.StartsWith("IL2", StringComparison.Ordinal) => "trim analysis warning",
        _ => "AOT analysis warning"
    };
}
