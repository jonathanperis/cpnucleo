namespace Infrastructure.Common.Mappings;

internal sealed class UserProjectMap : IEntityTypeConfiguration<UserProject>
{
    internal static List<UserProject>? UserProjects { get; set; }

    public void Configure(EntityTypeBuilder<UserProject> builder)
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
            .HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId);

        if (UserProjects != null)
        {
            builder.HasData(UserProjects);
        }
    }
}
