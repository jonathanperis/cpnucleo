namespace Infrastructure.Common.Helpers;

//For EF Core Ulid Integration
public sealed class UlidToBytesConverter(ConverterMappingHints mappingHints = null) : ValueConverter<Ulid, byte[]>(
            convertToProviderExpression: x => x.ToByteArray(),
            convertFromProviderExpression: x => new Ulid(x),
            mappingHints: defaultHints.With(mappingHints))
{
    private static readonly ConverterMappingHints defaultHints = new(size: 16);

    public UlidToBytesConverter() : this(null)
    {
    }
}
