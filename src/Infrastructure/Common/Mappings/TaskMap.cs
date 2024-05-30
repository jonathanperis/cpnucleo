namespace Infrastructure.Common.Mappings;

internal sealed class TaskMap : IEntityTypeConfiguration<Domain.Task>
{
    internal static List<Domain.Task>? Tasks { get; set; }

    public void Configure(EntityTypeBuilder<Domain.Task> builder)
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
            .HasOne(x => x.TaskType)
            .WithMany()
            .HasForeignKey(x => x.TaskTypeId);

        if (Tasks != null)
        {
            builder.HasData(Tasks);
        }
    }
}
