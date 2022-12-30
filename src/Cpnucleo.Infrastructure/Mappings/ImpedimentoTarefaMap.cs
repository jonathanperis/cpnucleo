namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class ImpedimentoTarefaMap : IEntityTypeConfiguration<ImpedimentoTarefa>
{
    internal static List<ImpedimentoTarefa> ImpedimentosTarefas { get; set; }

    public void Configure(EntityTypeBuilder<ImpedimentoTarefa> builder)
    {
        builder
            .ToTable("ImpedimentosTarefas", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Descricao)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(c => c.IdTarefa)
            .IsRequired();

        builder.Property(c => c.IdImpedimento)
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

        builder
            .HasOne(p => p.Impedimento)
            .WithMany()
            .HasForeignKey(f => f.IdImpedimento);

        if (ImpedimentosTarefas != null)
        {
            builder.HasData(ImpedimentosTarefas);
        }
    }
}
