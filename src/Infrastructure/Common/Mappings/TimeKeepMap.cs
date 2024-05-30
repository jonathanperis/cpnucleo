namespace Infrastructure.Common.Mappings;

internal sealed class TimeKeepMap : IEntityTypeConfiguration<TimeKeep>
{
    internal static List<TimeKeep>? TimeKeeps { get; set; }

    public void Configure(EntityTypeBuilder<TimeKeep> builder)
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
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);            

        if (TimeKeeps != null)
        {
            builder.HasData(TimeKeeps);
        }
    }
}
