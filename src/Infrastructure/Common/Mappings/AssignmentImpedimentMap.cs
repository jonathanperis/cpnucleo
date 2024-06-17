namespace Infrastructure.Common.Mappings;

internal sealed class AssignmentImpedimentMap : IEntityTypeConfiguration<AssignmentImpediment>
{
    internal static List<AssignmentImpediment>? AssignmentImpediments { get; set; }

    public void Configure(EntityTypeBuilder<AssignmentImpediment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        builder
            .HasOne(x => x.Assignment)
            .WithMany()
            .HasForeignKey(x => x.AssignmentId);

        builder
            .HasOne(x => x.Impediment)
            .WithMany()
            .HasForeignKey(x => x.ImpedimentId);

        if (AssignmentImpediments != null)
        {
            builder.HasData(AssignmentImpediments);
        }
    }
}
