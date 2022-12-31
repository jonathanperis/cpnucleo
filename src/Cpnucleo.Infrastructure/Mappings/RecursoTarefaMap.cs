namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class RecursoTarefaMap : IEntityTypeConfiguration<RecursoTarefa>
{
    internal static List<RecursoTarefa> RecursosTarefas { get; set; }

    public void Configure(EntityTypeBuilder<RecursoTarefa> builder)
    {
        builder
            .ToTable("RecursosTarefas", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.PercentualTarefa)
            .IsRequired();

        builder.Property(c => c.IdRecurso)
            .IsRequired();

        builder.Property(c => c.IdTarefa)
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .IsRequired();

        builder.Property(c => c.DataExclusao);

        builder.Property(c => c.Ativo)
            .IsRequired();

        builder
            .HasKey(nameof(BaseEntity.Id));

        builder
            .HasIndex(nameof(BaseEntity.ClusteredKey));

        builder
            .HasOne(p => p.Recurso)
            .WithMany()
            .HasForeignKey(f => f.IdRecurso);

        builder
            .HasOne(p => p.Tarefa)
            .WithMany()
            .HasForeignKey(f => f.IdTarefa);

        builder
        .Ignore(c => c.DataAlteracao);

        if (RecursosTarefas != null)
        {
            builder.HasData(RecursosTarefas);
        }
    }
}
