namespace Infrastructure.Common.Mappings;

internal sealed class ImpedimentMap : IEntityTypeConfiguration<Impediment>
{
    public void Configure(EntityTypeBuilder<Impediment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);
    }
}
