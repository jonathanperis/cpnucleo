namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class TipoTarefaMap : IEntityTypeConfiguration<TipoTarefa>
{
    public void Configure(EntityTypeBuilder<TipoTarefa> builder)
    {
        builder
            .ToTable("TiposTarefas", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(e => e.ClusteredKey)
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
            .HasKey(nameof(BaseEntity.Id))
            .IsClustered(false);

        builder
            .HasIndex(nameof(BaseEntity.ClusteredKey))
            .IsClustered(true);

        builder
            .Ignore(c => c.Element);
    }
}
