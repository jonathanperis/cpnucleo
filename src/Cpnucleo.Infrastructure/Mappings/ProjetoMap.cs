namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class ProjetoMap : IEntityTypeConfiguration<Projeto>
{
    internal static List<Projeto> Projetos { get; set; }

    public void Configure(EntityTypeBuilder<Projeto> builder)
    {
        builder
            .ToTable("Projetos", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder
            .Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.IdSistema)
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
            .HasOne(p => p.Sistema)
            .WithMany()
            .HasForeignKey(f => f.IdSistema);

        if (Projetos != null)
        {
            builder.HasData(Projetos);
        }
    }
}
