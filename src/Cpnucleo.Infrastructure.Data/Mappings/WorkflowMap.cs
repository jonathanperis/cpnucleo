namespace Cpnucleo.Infrastructure.Data.Mappings;

internal sealed class WorkflowMap : IEntityTypeConfiguration<Workflow>
{
    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        builder.ToTable("CPN_TB_WORKFLOW");

        builder.Property(c => c.Id)
            .HasColumnName("WOR_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.Nome)
            .HasColumnName("WOR_NOME")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Ordem)
            .HasColumnName("WOR_ORDEM")
            .HasColumnType("int")
            .IsRequired();

        builder
            .Ignore(c => c.TamanhoColuna);

        builder.Property(c => c.DataInclusao)
            .HasColumnName("WOR_DATA_INCLUSAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.DataAlteracao)
            .HasColumnName("WOR_DATA_ALTERACAO")
            .HasColumnType("datetime");

        builder.Property(c => c.DataExclusao)
            .HasColumnName("WOR_DATA_EXCLUSAO")
            .HasColumnType("datetime");

        builder.Property(c => c.Ativo)
            .HasColumnName("WOR_ATIVO")
            .HasColumnType("bit")
            .IsRequired();
    }
}
