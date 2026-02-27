using NJsonSchema.Generation;

namespace WebApi.ServiceExtensions;

internal sealed class SchemaNameGenerator : ISchemaNameGenerator
{
    public string Generate(Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var backtickIndex = type.Name.IndexOf('`');
        var baseName = backtickIndex >= 0 ? type.Name[..backtickIndex] : type.Name;
        var typeArgs = string.Join("Of", type.GetGenericArguments().Select(Generate));
        return $"{baseName}Of{typeArgs}";
    }
}
