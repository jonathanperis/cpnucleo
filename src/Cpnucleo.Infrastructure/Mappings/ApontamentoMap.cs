namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class ApontamentoMap : IEntityTypeConfiguration<Apontamento>
{
    public void Configure(EntityTypeBuilder<Apontamento> builder)
    {
        builder
            .ToTable("Apontamentos", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Descricao)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(c => c.DataApontamento)
            .IsRequired();

        builder.Property(c => c.QtdHoras)
            .IsRequired();

        builder.Property(c => c.IdTarefa)
            .IsRequired();

        builder.Property(c => c.IdRecurso)
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
            .HasOne(p => p.Tarefa)
            .WithMany()
            .HasForeignKey(f => f.IdTarefa);
    }
}
