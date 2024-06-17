namespace Infrastructure.Common.Mappings;

internal sealed class AppointmentMap : IEntityTypeConfiguration<Appointment>
{
    internal static List<Appointment>? Appointments { get; set; }

    public void Configure(EntityTypeBuilder<Appointment> builder)
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
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId);            

        if (Appointments != null)
        {
            builder.HasData(Appointments);
        }
    }
}
