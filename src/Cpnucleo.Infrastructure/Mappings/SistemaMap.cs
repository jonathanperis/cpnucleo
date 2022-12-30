namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class SistemaMap : IEntityTypeConfiguration<Sistema>
{
    internal static List<Sistema> Sistemas { get; set; }

    public void Configure(EntityTypeBuilder<Sistema> builder)
    {
        builder
            .ToTable("Sistemas", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .IsRequired();

        builder.Property(c => c.DataAlteracao);

        builder.Property(c => c.DataExclusao);

        builder.Property(c => c.Ativo)
            .IsRequired();

        builder
            .HasKey(nameof(BaseEntity.Id))
            .IsClustered(false);

        builder
            .HasIndex(nameof(BaseEntity.ClusteredKey))
            .IsClustered(true);

        if (Sistemas != null)
        {
            builder.HasData(Sistemas);
        }
    }
}
