namespace Infrastructure.Common.Mappings;

internal sealed class AssignmentTypeMap : IEntityTypeConfiguration<AssignmentType>
{
    internal static List<AssignmentType>? AssignmentTypes { get; set; }

    public void Configure(EntityTypeBuilder<AssignmentType> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        if (AssignmentTypes != null)
        {
            builder.HasData(AssignmentTypes);
        }
    }
}
