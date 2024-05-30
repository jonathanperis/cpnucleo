namespace Infrastructure.Common.Mappings;

internal sealed class SystemMap : IEntityTypeConfiguration<Domain.System>
{
    internal static List<Domain.System>? Systems { get; set; }

    public void Configure(EntityTypeBuilder<Domain.System> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        if (Systems != null)
        {
            builder.HasData(Systems);
        }
    }
}
