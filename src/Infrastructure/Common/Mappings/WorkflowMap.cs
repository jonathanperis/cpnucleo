namespace Infrastructure.Common.Mappings;

internal sealed class WorkflowMap : IEntityTypeConfiguration<Workflow>
{
    internal static List<Workflow>? Workflows { get; set; }

    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        if (Workflows != null)
        {
            builder.HasData(Workflows);
        }
    }
}
