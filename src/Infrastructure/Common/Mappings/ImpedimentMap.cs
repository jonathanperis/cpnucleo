namespace Infrastructure.Common.Mappings;

internal sealed class ImpedimentMap : IEntityTypeConfiguration<Impediment>
{
    internal static List<Impediment>? Impediments { get; set; }

    public void Configure(EntityTypeBuilder<Impediment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        if (Impediments != null)
        {
            builder.HasData(Impediments);
        }
    }
}
