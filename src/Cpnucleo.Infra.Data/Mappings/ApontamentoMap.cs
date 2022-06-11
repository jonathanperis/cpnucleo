namespace Cpnucleo.Infra.Data.Mappings;

internal class ApontamentoMap : IEntityTypeConfiguration<Apontamento>
{
    internal static List<Apontamento> Apontamentos { get; set; }

    public void Configure(EntityTypeBuilder<Apontamento> builder)
    {
        builder.ToTable("CPN_TB_APONTAMENTO");

        builder.Property(c => c.Id)
            .HasColumnName("APT_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasColumnName("APT_DESCRICAO")
            .HasColumnType("varchar(450)")
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(c => c.DataApontamento)
            .HasColumnName("APT_DATA_LANCAMENTO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.QtdHoras)
            .HasColumnName("APT_QTD_HORAS")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(c => c.IdTarefa)
            .HasColumnName("TAR_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.IdRecurso)
            .HasColumnName("REC_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .HasColumnName("APT_DATA_INCLUSAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.DataAlteracao)
            .HasColumnName("APT_DATA_ALTERACAO")
            .HasColumnType("datetime");

        builder.Property(c => c.DataExclusao)
            .HasColumnName("APT_DATA_EXCLUSAO")
            .HasColumnType("datetime");

        builder.Property(c => c.Ativo)
            .HasColumnName("APT_ATIVO")
            .HasColumnType("bit")
            .IsRequired();

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
