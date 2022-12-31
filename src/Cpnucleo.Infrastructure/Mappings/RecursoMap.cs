namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class RecursoMap : IEntityTypeConfiguration<Recurso>
{
    internal static List<Recurso> Recursos { get; set; }

    public void Configure(EntityTypeBuilder<Recurso> builder)
    {
        builder
            .ToTable("Recursos", "public");

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(e => e.ClusteredKey)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Login)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Senha)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(c => c.Salt)
            .HasMaxLength(64)
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
            .Ignore(c => c.Token);

        if (Recursos != null)
        {
            builder.HasData(Recursos);
        }
    }
}
