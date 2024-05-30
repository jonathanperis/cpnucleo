namespace Infrastructure.Common.Mappings;

internal sealed class TaskTypeMap : IEntityTypeConfiguration<TaskType>
{
    internal static List<TaskType>? TaskTypes { get; set; }

    public void Configure(EntityTypeBuilder<TaskType> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        if (TaskTypes != null)
        {
            builder.HasData(TaskTypes);
        }
    }
}
