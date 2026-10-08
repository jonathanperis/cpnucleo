using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace OpenApiLab;

/// <summary>
/// WebApi documents example values as <c>[DefaultValue("80aee820-...")]</c> strings on <see cref="Guid"/> and
/// <see cref="DateTime"/> properties. NSwag writes the string as-is; Microsoft.AspNetCore.OpenApi serializes
/// the attribute value with the property's own JSON contract and throws <see cref="InvalidCastException"/>.
/// This contract modifier converts such values to the property type before the schema exporter reads them.
/// It changes schema metadata only (serialization is unaffected) and is the lab's workaround, not a WebApi change.
/// </summary>
internal static class DefaultValueAlignment
{
    public static void Modify(JsonTypeInfo typeInfo)
    {
        foreach (var property in typeInfo.Properties)
        {
            if (property.AttributeProvider is not PropertyInfo info ||
                info.GetCustomAttribute<DefaultValueAttribute>() is not { Value: string text })
                continue;
            var target = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (target == typeof(string) || target == typeof(object)) continue;
            var converted = TypeDescriptor.GetConverter(target).ConvertFromString(null, CultureInfo.InvariantCulture, text);
            property.AttributeProvider = new AlignedProperty(info, new DefaultValueAttribute(converted));
        }
    }

    /// <summary>
    /// The original property with one replaced attribute. It stays a <see cref="PropertyInfo"/> so that
    /// metadata keyed by the member (XML documentation comments) still resolves.
    /// </summary>
    private sealed class AlignedProperty(PropertyInfo inner, DefaultValueAttribute replacement) : PropertyInfo
    {
        public override PropertyAttributes Attributes => inner.Attributes;
        public override bool CanRead => inner.CanRead;
        public override bool CanWrite => inner.CanWrite;
        public override Type PropertyType => inner.PropertyType;
        public override Type? DeclaringType => inner.DeclaringType;
        public override string Name => inner.Name;
        public override Type? ReflectedType => inner.ReflectedType;
        public override Module Module => inner.Module;
        public override int MetadataToken => inner.MetadataToken;
        public override MethodInfo[] GetAccessors(bool nonPublic) => inner.GetAccessors(nonPublic);
        public override MethodInfo? GetGetMethod(bool nonPublic) => inner.GetGetMethod(nonPublic);
        public override MethodInfo? GetSetMethod(bool nonPublic) => inner.GetSetMethod(nonPublic);
        public override ParameterInfo[] GetIndexParameters() => inner.GetIndexParameters();
        public override IList<CustomAttributeData> GetCustomAttributesData() => inner.GetCustomAttributesData();

        public override object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture) =>
            inner.GetValue(obj, invokeAttr, binder, index, culture);

        public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture) =>
            inner.SetValue(obj, value, invokeAttr, binder, index, culture);

        public override bool IsDefined(Type attributeType, bool inherit) => inner.IsDefined(attributeType, inherit);
        public override object[] GetCustomAttributes(bool inherit) => Replace(inner.GetCustomAttributes(inherit), typeof(object));

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) =>
            Replace(inner.GetCustomAttributes(attributeType, inherit), attributeType);

        private object[] Replace(object[] attributes, Type elementType)
        {
            // Reflection returns arrays typed as the requested attribute type; callers may rely on that.
            var result = (object[])Array.CreateInstance(elementType, attributes.Length);
            for (var i = 0; i < attributes.Length; i++)
                result[i] = attributes[i] is DefaultValueAttribute ? replacement : attributes[i];
            return result;
        }
    }
}
