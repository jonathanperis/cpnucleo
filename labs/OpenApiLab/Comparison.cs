using System.Globalization;
using System.Text.Json.Nodes;

namespace OpenApiLab;

/// <summary>
/// Compares a generated document with the committed NSwag document and prints one structured section.
/// Only the operation set is a claimed property (returned to the caller); everything else is reported.
/// </summary>
internal sealed class Comparison(JsonObject nswag, JsonObject candidate)
{
    private static readonly string[] Verbs = ["get", "put", "post", "delete", "patch", "head", "options"];
    private static readonly string[] ConstraintKeys =
        ["type", "format", "minLength", "maxLength", "minItems", "maxItems", "minimum", "maximum", "exclusiveMinimum", "exclusiveMaximum", "pattern", "enum", "nullable", "default"];
    private const string Envelope = "#/components/schemas/ApiErrorResponse";
    private const int NamesPerLine = 6;
    private const int MaximumNames = 18;

    private readonly Dictionary<string, JsonObject> nswagOps = Operations(nswag);
    private readonly Dictionary<string, JsonObject> candidateOps = Operations(candidate);

    /// <summary>Prints the report; returns whether both documents describe the same operations (path + verb).</summary>
    public bool Report()
    {
        var shared = nswagOps.Keys.Intersect(candidateOps.Keys).Order(StringComparer.Ordinal).ToList();
        var onlyNswag = nswagOps.Keys.Except(candidateOps.Keys).Order(StringComparer.Ordinal).ToList();
        var onlyCandidate = candidateOps.Keys.Except(nswagOps.Keys).Order(StringComparer.Ordinal).ToList();
        var sameOperations = onlyNswag.Count == 0 && onlyCandidate.Count == 0;

        Line("Document", $"openapi {nswag["openapi"]} (NSwag) vs {candidate["openapi"]}; info fields equal: {InfoSummary()}");
        Line("Operations", $"both {shared.Count}, only NSwag {onlyNswag.Count}, only FE.OpenApi {onlyCandidate.Count}  [asserted: {(sameOperations ? "PASS" : "FAIL")}]");
        Detail("only NSwag", onlyNswag);
        Detail("only FE.OpenApi", onlyCandidate);

        Line("OperationIds", Ratio(shared, key => Text(nswagOps[key]["operationId"]) == Text(candidateOps[key]["operationId"])) + " equal");
        Detail("differs", shared.Where(key => Text(nswagOps[key]["operationId"]) != Text(candidateOps[key]["operationId"]))
            .Select(key => $"{key}: {Text(nswagOps[key]["operationId"])} -> {Text(candidateOps[key]["operationId"])}"));

        Line("Tags", $"per operation {Ratio(shared, key => Strings(nswagOps[key]["tags"]).SequenceEqual(Strings(candidateOps[key]["tags"])))} equal; " +
            $"document tags with equal description: {DocumentTags()}");
        Line("Security", $"schemes NSwag [{string.Join(", ", SchemeNames(nswag))}] FE.OpenApi [{string.Join(", ", SchemeNames(candidate))}]; " +
            $"per-operation requirement equal {Ratio(shared, key => Canonical(nswagOps[key]["security"]) == Canonical(candidateOps[key]["security"]))}");

        ReportResponses(shared);
        ReportListQueries(shared);
        ReportSchemas();
        ReportRequestBodies(shared);

        ReportDescriptions();
        return sameOperations;
    }

    /// <summary>XML documentation comments: properties of same-named schemas that NSwag describes and the candidate does not.</summary>
    private void ReportDescriptions()
    {
        var expected = nswag["components"]?["schemas"] as JsonObject ?? [];
        var actual = candidate["components"]?["schemas"] as JsonObject ?? [];
        var lost = (from schema in expected
                    where actual[schema.Key] is JsonObject
                    from property in schema.Value?["properties"] as JsonObject ?? []
                    where property.Value?["description"] is not null && actual[schema.Key]!["properties"]?[property.Key]?["description"] is null
                    select property.Value).ToList();
        var references = lost.Count(property => property?["oneOf"] is not null || property?["allOf"] is not null || property?["$ref"] is not null);
        Line("XML docs", $"operation parameters described NSwag {DescribedParameters(nswagOps)}, FE.OpenApi {DescribedParameters(candidateOps)}; " +
            $"properties of same-named schemas described by NSwag but not FE.OpenApi: {lost.Count} ({references} of them reference another schema)");
    }

    private void ReportResponses(List<string> shared)
    {
        Line("Responses", $"identical status-code sets on {Ratio(shared, key => Statuses(nswagOps[key]).SetEquals(Statuses(candidateOps[key])))} operations");
        var patterns = shared
            .Select(key => (Missing: Statuses(nswagOps[key]).Except(Statuses(candidateOps[key])).Order().ToList(),
                Extra: Statuses(candidateOps[key]).Except(Statuses(nswagOps[key])).Order().ToList()))
            .Where(diff => diff.Missing.Count + diff.Extra.Count > 0)
            .GroupBy(diff => $"FE.OpenApi lacks [{string.Join(" ", diff.Missing)}]" + (diff.Extra.Count > 0 ? $", adds [{string.Join(" ", diff.Extra)}]" : ""))
            .OrderByDescending(group => group.Count())
            .Select(group => $"{group.Count(),3} ops: {group.Key}");
        Detail("pattern", patterns);
        var sharedStatuses = shared.SelectMany(key => Statuses(nswagOps[key]).Intersect(Statuses(candidateOps[key])).Select(status => (key, status))).ToList();
        var sameDescriptions = sharedStatuses.Count(pair =>
            Text(nswagOps[pair.key]["responses"]![pair.status]!["description"]) == Text(candidateOps[pair.key]["responses"]![pair.status]!["description"]));
        Line("", $"response descriptions equal on {sameDescriptions}/{sharedStatuses.Count} statuses present in both");
        Line("Envelope", $"error responses (4xx/5xx) referencing ApiErrorResponse: NSwag {EnvelopeRatio(nswagOps)}, FE.OpenApi {EnvelopeRatio(candidateOps)}");
    }

    private void ReportListQueries(List<string> shared)
    {
        var lists = shared.Where(key => Text(nswagOps[key]["operationId"])?.StartsWith("List", StringComparison.Ordinal) == true).ToList();
        var sameKeys = lists.Count(key => QueryParameters(nswagOps[key]).Keys.ToHashSet().SetEquals(QueryParameters(candidateOps[key]).Keys));
        var sameSchemas = lists.Count(key =>
        {
            var expected = QueryParameters(nswagOps[key]);
            var actual = QueryParameters(candidateOps[key]);
            return expected.Count == actual.Count && expected.All(pair => actual.TryGetValue(pair.Key, out var schema) && schema == pair.Value);
        });
        Line("List queries", $"{lists.Count} list operations; same flat query keys on {sameKeys}/{lists.Count}, same keys and schemas on {sameSchemas}/{lists.Count}");
        var differences = new List<string>();
        foreach (var key in lists)
        {
            var expected = QueryParameters(nswagOps[key]);
            var actual = QueryParameters(candidateOps[key]);
            differences.AddRange(expected.Keys.Except(actual.Keys).Select(name => $"missing {name}"));
            // The API answers 400 to a relation/date filter the listed resource lacks.
            differences.AddRange(actual.Keys.Except(expected.Keys).Select(name => $"advertises unsupported filter {name}"));
            differences.AddRange(expected.Where(pair => actual.TryGetValue(pair.Key, out var schema) && schema != pair.Value)
                .Select(pair => $"{pair.Key} {pair.Value} -> {actual[pair.Key]}"));
        }
        Detail("pattern", differences.GroupBy(difference => difference).OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key, StringComparer.Ordinal).Select(group => $"{group.Count(),2}/{lists.Count} lists: {group.Key}"), limit: 14);
    }

    private void ReportSchemas()
    {
        var expected = SchemaNames(nswag);
        var actual = SchemaNames(candidate);
        Line("Schemas", $"NSwag {expected.Count}, FE.OpenApi {actual.Count}; same name {expected.Intersect(actual).Count()}, " +
            $"only NSwag {expected.Except(actual).Count()}, only FE.OpenApi {actual.Except(expected).Count()}; " +
            $"additionalProperties:false on NSwag {ClosedSchemas(nswag)}, FE.OpenApi {ClosedSchemas(candidate)}");
        Detail("only NSwag", Wrap(expected.Except(actual)));
        Detail("only FE.OpenApi", Wrap(actual.Except(expected)));
    }

    /// <summary>Up to <see cref="MaximumNames"/> schema names in a few comma-separated lines.</summary>
    private static IEnumerable<string> Wrap(IEnumerable<string> names)
    {
        var sorted = names.Order(StringComparer.Ordinal).ToList();
        foreach (var chunk in sorted.Take(MaximumNames).Chunk(NamesPerLine)) yield return string.Join(", ", chunk);
        if (sorted.Count > MaximumNames) yield return $"... and {sorted.Count - MaximumNames} more";
    }

    private void ReportRequestBodies(List<string> shared)
    {
        var bodies = shared.Where(key => nswagOps[key]["requestBody"] is not null && candidateOps[key]["requestBody"] is not null).ToList();
        var requiredEqual = 0;
        var differences = new List<string>();
        foreach (var key in bodies)
        {
            var (expectedRequired, expectedProperties) = BodySchema(nswag, nswagOps[key]);
            var (actualRequired, actualProperties) = BodySchema(candidate, candidateOps[key]);
            if (expectedRequired.SetEquals(actualRequired)) requiredEqual++;
            foreach (var (property, expectedSchema) in expectedProperties)
            {
                if (!actualProperties.TryGetValue(property, out var actualSchema))
                {
                    differences.Add("property missing in FE.OpenApi");
                    continue;
                }
                var type = Text(expectedSchema["type"]) ?? "object";
                foreach (var constraint in ConstraintKeys)
                {
                    var before = Constraint(expectedSchema, constraint);
                    var after = Constraint(actualSchema, constraint);
                    if (before != after) differences.Add($"{constraint} on {type}: {before ?? "(none)"} -> {after ?? "(none)"}");
                }
            }
        }
        Line("Request bodies", $"{bodies.Count} JSON bodies; required-property sets equal {requiredEqual}/{bodies.Count}; " +
            $"property constraint differences {differences.Count} (NSwag -> FE.OpenApi):");
        Detail("pattern", differences.GroupBy(difference => difference).OrderByDescending(group => group.Count())
            .Select(group => $"{group.Count(),3}x {group.Key}"));
    }

    private string InfoSummary()
    {
        var expected = nswag["info"]!.AsObject();
        var actual = candidate["info"]!.AsObject();
        var fields = expected.Select(pair => pair.Key).Union(actual.Select(pair => pair.Key)).Order(StringComparer.Ordinal).ToList();
        var equal = fields.Where(field => Canonical(expected[field]) == Canonical(actual[field])).ToList();
        return $"{equal.Count}/{fields.Count}" + (equal.Count == fields.Count ? "" : $" (differ: {string.Join(", ", fields.Except(equal))})");
    }

    private string DocumentTags()
    {
        var expected = Tags(nswag);
        var actual = Tags(candidate);
        return $"{expected.Count(pair => actual.TryGetValue(pair.Key, out var description) && description == pair.Value)}/{expected.Count} " +
            $"(FE.OpenApi lists {actual.Count})";
    }

    private static Dictionary<string, string?> Tags(JsonObject document) =>
        (document["tags"] as JsonArray ?? []).OfType<JsonObject>().ToDictionary(tag => Text(tag["name"])!, tag => Text(tag["description"]));

    private static Dictionary<string, JsonObject> Operations(JsonObject document)
    {
        var operations = new Dictionary<string, JsonObject>(StringComparer.Ordinal);
        foreach (var (path, item) in document["paths"]!.AsObject())
            foreach (var verb in Verbs.Where(verb => item?[verb] is JsonObject))
                operations[$"{verb.ToUpperInvariant()} {path}"] = (JsonObject)item![verb]!;
        return operations;
    }

    private static HashSet<string> Statuses(JsonObject operation) =>
        (operation["responses"] as JsonObject ?? []).Select(pair => pair.Key).ToHashSet(StringComparer.Ordinal);

    private static string EnvelopeRatio(Dictionary<string, JsonObject> operations)
    {
        var errors = operations.Values
            .SelectMany(operation => (operation["responses"] as JsonObject ?? []).Where(pair => pair.Key[0] is '4' or '5'))
            .ToList();
        var enveloped = errors.Count(pair => Text(pair.Value?["content"]?["application/json"]?["schema"]?["$ref"]) == Envelope);
        return $"{enveloped}/{errors.Count}";
    }

    private static SortedDictionary<string, string> QueryParameters(JsonObject operation) =>
        new((operation["parameters"] as JsonArray ?? []).OfType<JsonObject>()
                .Where(parameter => Text(parameter["in"]) == "query")
                .ToDictionary(parameter => Text(parameter["name"])!, parameter => Describe(parameter["schema"] as JsonObject)),
            StringComparer.Ordinal);

    /// <summary>The constraint-bearing keys of a parameter schema in one comparable line.</summary>
    private static string Describe(JsonObject? schema) =>
        "{" + string.Join(", ", ConstraintKeys.Select(key => (key, value: Constraint(schema, key))).Where(pair => pair.value is not null)
            .Select(pair => $"{pair.key}={pair.value}")) + "}";

    private static string? Constraint(JsonObject? schema, string key)
    {
        var value = schema?[key];
        // NSwag spells out "nullable": false; an absent nullable means the same.
        if (key == "nullable" && value is null) return "false";
        if (value is JsonValue number && number.TryGetValue<decimal>(out var numeric)) return numeric.ToString("G29", CultureInfo.InvariantCulture);
        return value is null ? null : Canonical(value);
    }

    private static HashSet<string> SchemaNames(JsonObject document) =>
        (document["components"]?["schemas"] as JsonObject ?? []).Select(pair => pair.Key).ToHashSet(StringComparer.Ordinal);

    private static int ClosedSchemas(JsonObject document) =>
        (document["components"]?["schemas"] as JsonObject ?? []).Count(pair => Descendants(pair.Value)
            .Any(node => node is JsonObject schema && schema["additionalProperties"] is JsonValue closed && closed.TryGetValue<bool>(out var open) && !open));

    private static IEnumerable<string> SchemeNames(JsonObject document) =>
        (document["components"]?["securitySchemes"] as JsonObject ?? []).Select(pair => $"{pair.Key}:{Text(pair.Value?["type"])}/{Text(pair.Value?["scheme"])}");

    private static int DescribedParameters(Dictionary<string, JsonObject> operations) =>
        operations.Values.SelectMany(operation => (operation["parameters"] as JsonArray ?? []).OfType<JsonObject>())
            .Count(parameter => parameter["description"] is not null);

    /// <summary>Required names and properties of a JSON request body, following $ref, allOf and single-item oneOf.</summary>
    private static (HashSet<string> Required, Dictionary<string, JsonObject> Properties) BodySchema(JsonObject document, JsonObject operation)
    {
        var required = new HashSet<string>(StringComparer.Ordinal);
        var properties = new Dictionary<string, JsonObject>(StringComparer.Ordinal);
        Collect(document, operation["requestBody"]?["content"]?["application/json"]?["schema"] as JsonObject, required, properties, depth: 0);
        return (required, properties);
    }

    private static void Collect(JsonObject document, JsonObject? schema, HashSet<string> required, Dictionary<string, JsonObject> properties, int depth)
    {
        if (schema is null || depth > 8) return;
        if (Text(schema["$ref"]) is { } reference)
        {
            Collect(document, document["components"]?["schemas"]?[reference.Split('/')[^1]] as JsonObject, required, properties, depth + 1);
            return;
        }
        foreach (var part in schema["allOf"] as JsonArray ?? []) Collect(document, part as JsonObject, required, properties, depth + 1);
        foreach (var name in Strings(schema["required"])) required.Add(name);
        foreach (var (name, property) in schema["properties"] as JsonObject ?? [])
            if (property is JsonObject propertySchema) properties[name] = Resolve(document, propertySchema);
    }

    /// <summary>A property schema with a single-reference oneOf/allOf wrapper replaced by its target's keys.</summary>
    private static JsonObject Resolve(JsonObject document, JsonObject schema)
    {
        var wrapped = (schema["oneOf"] ?? schema["allOf"]) as JsonArray;
        if (Text(schema["$ref"]) is null && wrapped is not [JsonObject single]) return schema;
        var reference = Text(schema["$ref"]) ?? Text(wrapped![0]!["$ref"]);
        if (reference is null || document["components"]?["schemas"]?[reference.Split('/')[^1]] is not JsonObject target) return schema;
        var merged = (JsonObject)target.DeepClone();
        foreach (var (key, value) in schema)
            if (key is not "$ref" and not "oneOf" and not "allOf") merged[key] = value?.DeepClone();
        return merged;
    }

    private static IEnumerable<JsonNode?> Descendants(JsonNode? node)
    {
        yield return node;
        var children = node switch
        {
            JsonObject properties => properties.Select(pair => pair.Value),
            JsonArray items => items.AsEnumerable(),
            _ => []
        };
        foreach (var descendant in children.SelectMany(Descendants)) yield return descendant;
    }

    private static string? Text(JsonNode? node) => node is JsonValue value && value.TryGetValue<string>(out var text) ? text : null;

    private static IEnumerable<string> Strings(JsonNode? node) => (node as JsonArray ?? []).Select(Text).OfType<string>();

    /// <summary>Key-order-insensitive JSON text (the committed snapshot is stored with sorted keys).</summary>
    private static string Canonical(JsonNode? node) => node switch
    {
        null => "null",
        JsonObject properties => "{" + string.Join(",", properties.OrderBy(pair => pair.Key, StringComparer.Ordinal)
            .Select(pair => $"\"{pair.Key}\":{Canonical(pair.Value)}")) + "}",
        JsonArray items => "[" + string.Join(",", items.Select(Canonical)) + "]",
        JsonValue number when number.TryGetValue<decimal>(out var numeric) => numeric.ToString("G29", CultureInfo.InvariantCulture),
        _ => node.ToJsonString()
    };

    private static string Ratio(List<string> keys, Func<string, bool> predicate) => $"{keys.Count(predicate)}/{keys.Count}";

    private static void Line(string topic, string text) => Console.WriteLine($"  {topic,-15} {text}");

    private static void Detail(string label, IEnumerable<string> items, int limit = 10)
    {
        var list = items.ToList();
        foreach (var item in list.Take(limit)) Console.WriteLine($"  {"",-15}   {label}: {item}");
        if (list.Count > limit) Console.WriteLine($"  {"",-15}   {label}: ... {list.Count - limit} more");
    }
}
