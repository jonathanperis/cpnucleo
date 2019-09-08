using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cpnucleo.Infra.Data.Mappings
{
    class TarefaMap : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("TAR_ID");

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
                .IsRequired();

            builder.Property(c => c.PercentualConcluido)
                .HasColumnName("TAR_PERCENTUAL_CONCLUIDO")
                .HasColumnType("int");

            builder.Property(c => c.IdProjeto)
                .HasColumnName("PROJ_ID")
                .HasColumnType("int");

            builder.Property(c => c.IdWorkflow)
                .HasColumnName("WOR_ID")
                .HasColumnType("int");

            builder.Property(c => c.IdRecurso)
                .HasColumnName("REC_ID")
                .HasColumnType("int");

            builder.Property(c => c.IdTipoTarefa)
                .HasColumnName("TIP_ID")
                .HasColumnType("int");

            builder.Property(c => c.DataInclusao)
                .HasColumnName("TAR_DATA_INCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("TAR_DATA_ALTERACAO")
                .HasColumnType("datetime");
        }
    }
}
