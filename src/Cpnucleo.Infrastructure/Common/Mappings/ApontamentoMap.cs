namespace Cpnucleo.Infrastructure.Common.Mappings;

internal sealed class ApontamentoMap : IEntityTypeConfiguration<Apontamento>
{
    internal static List<Apontamento>? Apontamentos { get; set; }

    public void Configure(EntityTypeBuilder<Apontamento> builder)
    {
        builder
            .ToTable("Apontamentos", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.ClusteredKey)
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
            .HasKey(nameof(BaseEntity.Id));

        builder
            .HasIndex(nameof(BaseEntity.ClusteredKey));

        builder
            .HasOne(p => p.Tarefa)
            .WithMany()
            .HasForeignKey(f => f.IdTarefa);

        if (Apontamentos != null)
        {
            builder.HasData(Apontamentos);
        }
    }
}
