namespace Cpnucleo.Infrastructure.Data.Mappings;

internal class SistemaMap : IEntityTypeConfiguration<Sistema>
{
    public void Configure(EntityTypeBuilder<Sistema> builder)
    {
        builder.ToTable("CPN_TB_SISTEMA");

        builder.Property(c => c.Id)
            .HasColumnName("SIS_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.Nome)
            .HasColumnName("SIS_NOME")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasColumnName("SIS_DESCRICAO")
            .HasColumnType("varchar(450)")
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .HasColumnName("SIS_DATA_INCLUSAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.DataAlteracao)
            .HasColumnName("SIS_DATA_ALTERACAO")
            .HasColumnType("datetime");

        builder.Property(c => c.DataExclusao)
            .HasColumnName("SIS_DATA_EXCLUSAO")
            .HasColumnType("datetime");

        builder.Property(c => c.Ativo)
            .HasColumnName("SIS_ATIVO")
            .HasColumnType("bit")
            .IsRequired();
    }
}
