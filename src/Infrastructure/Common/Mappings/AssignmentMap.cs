namespace Infrastructure.Common.Mappings;

internal sealed class AssignmentMap : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        builder
            .HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId);

        builder
            .HasOne(x => x.Workflow)
            .WithMany()
            .HasForeignKey(x => x.WorkflowId);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder
            .HasOne(x => x.AssignmentType)
            .WithMany()
            .HasForeignKey(x => x.AssignmentTypeId);
    }
}
