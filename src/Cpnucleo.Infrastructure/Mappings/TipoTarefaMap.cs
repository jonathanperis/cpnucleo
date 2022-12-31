namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class TipoTarefaMap : IEntityTypeConfiguration<TipoTarefa>
{
    internal static List<TipoTarefa> TiposTarefas { get; set; }

    public void Configure(EntityTypeBuilder<TipoTarefa> builder)
    {
        builder
            .ToTable("TiposTarefas", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Image)
            .HasMaxLength(100)
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

        builder
        .Ignore(c => c.Element);

        if (TiposTarefas != null)
        {
            builder.HasData(TiposTarefas);
        }
    }
}
