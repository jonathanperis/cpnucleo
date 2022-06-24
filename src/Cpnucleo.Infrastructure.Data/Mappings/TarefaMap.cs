namespace Cpnucleo.Infrastructure.Data.Mappings;

internal class TarefaMap : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("CPN_TB_TAREFA");

        builder.Property(c => c.Id)
            .HasColumnName("TAR_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.Nome)
            .HasColumnName("TAR_NOME")
            .HasColumnType("varchar(450)")
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(c => c.DataInicio)
            .HasColumnName("TAR_DATA_INICIO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.DataTermino)
            .HasColumnName("TAR_DATA_TERMINO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.QtdHoras)
            .HasColumnName("TAR_QTD_HORAS")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(c => c.Detalhe)
            .HasColumnName("TAR_DETALHE")
            .HasColumnType("varchar(1000)")
            .HasMaxLength(1000);

        builder.Property(c => c.IdProjeto)
            .HasColumnName("PROJ_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.IdWorkflow)
            .HasColumnName("WOR_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.IdRecurso)
            .HasColumnName("REC_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.IdTipoTarefa)
            .HasColumnName("TIP_ID")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(c => c.DataInclusao)
            .HasColumnName("TAR_DATA_INCLUSAO")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.DataAlteracao)
            .HasColumnName("TAR_DATA_ALTERACAO")
            .HasColumnType("datetime");

        builder.Property(c => c.DataExclusao)
            .HasColumnName("TAR_DATA_EXCLUSAO")
            .HasColumnType("datetime");

        builder.Property(c => c.Ativo)
            .HasColumnName("TAR_ATIVO")
            .HasColumnType("bit")
            .IsRequired();

        builder
            .HasOne(p => p.Projeto)
            .WithMany()
            .HasForeignKey(f => f.IdProjeto);

        builder
            .HasOne(p => p.Workflow)
            .WithMany()
            .HasForeignKey(f => f.IdWorkflow);

        builder
            .HasOne(p => p.Recurso)
            .WithMany()
            .HasForeignKey(f => f.IdRecurso);

        builder
            .HasOne(p => p.TipoTarefa)
            .WithMany()
            .HasForeignKey(f => f.IdTipoTarefa);

        builder
            .Ignore(c => c.HorasConsumidas);

        builder
            .Ignore(c => c.HorasRestantes);
    }
}
