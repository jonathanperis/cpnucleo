namespace Infrastructure.Common.Helpers;

public class UlidToBytesConverter(ConverterMappingHints mappingHints = null) : ValueConverter<Ulid, byte[]>(
            convertToProviderExpression: x => x.ToByteArray(),
            convertFromProviderExpression: x => new Ulid(x),
            mappingHints: defaultHints.With(mappingHints))
{
    private static readonly ConverterMappingHints defaultHints = new ConverterMappingHints(size: 16);

    public UlidToBytesConverter() : this(null)
    {
    }
}