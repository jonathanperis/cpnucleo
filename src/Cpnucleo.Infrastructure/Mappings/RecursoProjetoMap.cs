namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class RecursoProjetoMap : IEntityTypeConfiguration<RecursoProjeto>
{
    internal static List<RecursoProjeto> RecursosProjetos { get; set; }

    public void Configure(EntityTypeBuilder<RecursoProjeto> builder)
    {
        builder
            .ToTable("RecursosProjetos", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.IdRecurso)
            .IsRequired();

        builder.Property(c => c.IdProjeto)
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .IsRequired();

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
            .HasOne(p => p.Recurso)
            .WithMany()
            .HasForeignKey(f => f.IdRecurso);

        builder
            .HasOne(p => p.Projeto)
            .WithMany()
            .HasForeignKey(f => f.IdProjeto);

        builder
        .Ignore(c => c.DataAlteracao);

        if (RecursosProjetos != null)
        {
            builder.HasData(RecursosProjetos);
        }
    }
}
