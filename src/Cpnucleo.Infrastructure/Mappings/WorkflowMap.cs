namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class WorkflowMap : IEntityTypeConfiguration<Workflow>
{
    internal static List<Workflow> Workflows { get; set; }

    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        builder
            .ToTable("Workflows", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Ordem)
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
            .Ignore(c => c.TamanhoColuna);

        if (Workflows != null)
        {
            builder.HasData(Workflows);
        }
    }
}
