namespace Infrastructure.Common.Mappings;

internal sealed class ProjectMap : IEntityTypeConfiguration<Project>
{
    internal static List<Project>? Projects { get; set; }

    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        builder
            .HasOne(x => x.Organization)
            .WithMany()
            .HasForeignKey(x => x.OrganizationId);

        if (Projects != null)
        {
            builder.HasData(Projects);
        }
    }
}
