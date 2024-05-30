namespace Infrastructure.Common.Mappings;

internal sealed class UserTaskMap : IEntityTypeConfiguration<UserTask>
{
    internal static List<UserTask>? UserTasks { get; set; }

    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);

        builder
            .HasOne(x => x.Task)
            .WithMany()
            .HasForeignKey(x => x.TaskId);

        if (UserTasks != null)
        {
            builder.HasData(UserTasks);
        }
    }
}
