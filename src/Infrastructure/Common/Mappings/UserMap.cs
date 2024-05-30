namespace Infrastructure.Common.Mappings;

internal sealed class UserMap : IEntityTypeConfiguration<User>
{
    internal static List<User>? Users { get; set; }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .HasIndex(x => x.CreatedAt);

        if (Users != null)
        {
            builder.HasData(Users);
        }
    }
}
