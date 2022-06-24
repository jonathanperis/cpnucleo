namespace Cpnucleo.Infrastructure.Data.Mappings;

internal class TipoTarefaMap : IEntityTypeConfiguration<TipoTarefa>
{
    public void Configure(EntityTypeBuilder<TipoTarefa> builder)
    {
        builder.ToTable("CPN_TB_TIPO_TAREFA");

        builder.Property(c => c.Id)
            .HasColumnName("TIP_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.Nome)
            .HasColumnName("TIP_NOME")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Image)
            .HasColumnName("TIP_IMAGE_CARD")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Ignore(c => c.Element);

        builder.Property(c => c.DataInclusao)
            .HasColumnName("TIP_DATA_INCLUSAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.DataAlteracao)
            .HasColumnName("TIP_DATA_ALTERACAO")
            .HasColumnType("datetime");

        builder.Property(c => c.DataExclusao)
            .HasColumnName("TIP_DATA_EXCLUSAO")
            .HasColumnType("datetime");

        builder.Property(c => c.Ativo)
            .HasColumnName("TIP_ATIVO")
            .HasColumnType("bit")
            .IsRequired();
    }
}
