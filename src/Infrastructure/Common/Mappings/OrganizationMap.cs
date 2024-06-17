namespace Infrastructure.Common.Mappings;

internal sealed class OrganizationMap : IEntityTypeConfiguration<Organization>
{
    internal static List<Organization>? Organizations { get; set; }

    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        if (Organizations != null)
        {
            builder.HasData(Organizations);
        }
    }
}
