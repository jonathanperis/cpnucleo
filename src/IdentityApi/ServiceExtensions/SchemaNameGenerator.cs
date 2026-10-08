using NJsonSchema.Generation;

namespace IdentityApi.ServiceExtensions;

internal sealed class SchemaNameGenerator : ISchemaNameGenerator
{
    public string Generate(Type type)
    {
        if (!type.IsGenericType)
        {
            // Every endpoint folder has its own Request/Response: prefix the feature (LoginRequest).
            return type is { Name: "Request" or "Response", Namespace: { } ns } && ns.StartsWith("IdentityApi.Endpoints.", StringComparison.Ordinal)
                ? ns[(ns.LastIndexOf('.') + 1)..] + type.Name
                : type.Name;
        }

        var backtickIndex = type.Name.IndexOf('`');
        var baseName = backtickIndex >= 0 ? type.Name[..backtickIndex] : type.Name;
        var typeArgs = string.Join("Of", type.GetGenericArguments().Select(Generate));
        return $"{baseName}Of{typeArgs}";
    }
}
