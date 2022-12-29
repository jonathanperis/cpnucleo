namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class TarefaMap : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder
            .ToTable("Tarefas", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(c => c.DataInicio)
            .IsRequired();

        builder.Property(c => c.DataTermino)
            .IsRequired();

        builder.Property(c => c.QtdHoras)
            .IsRequired();

        builder.Property(c => c.Detalhe)
            .HasMaxLength(1000);

        builder.Property(c => c.IdProjeto)
            .IsRequired();

        builder.Property(c => c.IdWorkflow)
            .IsRequired();

        builder.Property(c => c.IdRecurso)
            .IsRequired();

        builder.Property(c => c.IdTipoTarefa)
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
            .HasOne(p => p.Projeto)
            .WithMany()
            .HasForeignKey(f => f.IdProjeto);

        builder
            .HasOne(p => p.Workflow)
            .WithMany()
            .HasForeignKey(f => f.IdWorkflow);

        builder
            .HasOne(p => p.Recurso)
            .WithMany()
            .HasForeignKey(f => f.IdRecurso);

        builder
            .HasOne(p => p.TipoTarefa)
            .WithMany()
            .HasForeignKey(f => f.IdTipoTarefa);

        builder
            .Ignore(c => c.HorasConsumidas);

        builder
            .Ignore(c => c.HorasRestantes);
    }
}
