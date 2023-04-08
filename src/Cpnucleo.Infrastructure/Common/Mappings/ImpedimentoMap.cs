namespace Cpnucleo.Infrastructure.Common.Mappings;

internal sealed class ImpedimentoMap : IEntityTypeConfiguration<Impedimento>
{
    internal static List<Impedimento>? Impedimentos { get; set; }

    public void Configure(EntityTypeBuilder<Impedimento> builder)
    {
        builder
            .ToTable("Impedimentos", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .IsRequired();

        builder.Property(c => c.DataAlteracao);

        builder.Property(c => c.DataExclusao);

        builder.Property(c => c.Ativo)
            .IsRequired();

        builder
            .HasKey(nameof(BaseEntity.Id));

        builder
            .HasIndex(nameof(BaseEntity.ClusteredKey));

        if (Impedimentos != null)
        {
            builder.HasData(Impedimentos);
        }
    }
}
