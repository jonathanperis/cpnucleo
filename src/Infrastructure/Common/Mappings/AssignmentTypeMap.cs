namespace Infrastructure.Common.Mappings;

internal sealed class AssignmentTypeMap : IEntityTypeConfiguration<AssignmentType>
{
    public void Configure(EntityTypeBuilder<AssignmentType> builder)
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
