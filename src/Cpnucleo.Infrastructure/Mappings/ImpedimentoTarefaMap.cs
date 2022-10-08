namespace Cpnucleo.Infrastructure.Mappings;

internal sealed class ImpedimentoTarefaMap : IEntityTypeConfiguration<ImpedimentoTarefa>
{
    public void Configure(EntityTypeBuilder<ImpedimentoTarefa> builder)
    {
        builder.ToTable("CPN_TB_TAREFA_IMPEDIMENTO");

        builder.Property(c => c.Id)
            .HasColumnName("ITAR_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.Descricao)
            .HasColumnName("ITAR_DESCRICAO")
            .HasColumnType("varchar(450)")
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(c => c.IdTarefa)
            .HasColumnName("TAR_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.IdImpedimento)
            .HasColumnName("IMP_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .HasColumnName("ITAR_DATA_INCLUSAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.DataAlteracao)
            .HasColumnName("ITAR_DATA_ALTERACAO")
            .HasColumnType("datetime");

        builder.Property(c => c.DataExclusao)
            .HasColumnName("ITAR_DATA_EXCLUSAO")
            .HasColumnType("datetime");

        builder.Property(c => c.Ativo)
            .HasColumnName("ITAR_ATIVO")
            .HasColumnType("bit")
            .IsRequired();

        builder
            .HasOne(p => p.Tarefa)
            .WithMany()
            .HasForeignKey(f => f.IdTarefa);

        builder
            .HasOne(p => p.Impedimento)
            .WithMany()
            .HasForeignKey(f => f.IdImpedimento);
    }
}
