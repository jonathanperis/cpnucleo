namespace Infrastructure.Common.Helpers;

public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    private static readonly ConverterMappingHints defaultHints = new ConverterMappingHints(size: 26);

    public UlidToStringConverter() : this(null)
    {
    }

    public UlidToStringConverter(ConverterMappingHints mappingHints = null)
        : base(
                convertToProviderExpression: x => x.ToString(),
                convertFromProviderExpression: x => Ulid.Parse(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}