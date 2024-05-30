namespace Infrastructure.Common.Mappings;

internal sealed class TaskImpedimentMap : IEntityTypeConfiguration<TaskImpediment>
{
    internal static List<TaskImpediment>? TaskImpediments { get; set; }

    public void Configure(EntityTypeBuilder<TaskImpediment> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        builder
            .HasOne(x => x.Task)
            .WithMany()
            .HasForeignKey(x => x.TaskId);

        builder
            .HasOne(x => x.Impediment)
            .WithMany()
            .HasForeignKey(x => x.ImpedimentId);

        if (TaskImpediments != null)
        {
            builder.HasData(TaskImpediments);
        }
    }
}
