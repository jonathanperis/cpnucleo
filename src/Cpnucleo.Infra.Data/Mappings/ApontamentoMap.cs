using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cpnucleo.Infra.Data.Mappings
{
    class ApontamentoMap : IEntityTypeConfiguration<Apontamento>
    {
        public void Configure(EntityTypeBuilder<Apontamento> builder)
        {
            builder.ToTable("CPN_TB_LANCAMENTO");

            builder.Property(c => c.Id)
                .HasColumnName("LANC_ID");

            builder.Property(c => c.Descricao)
                .HasColumnName("LANC_DESCRICAO")
                .HasColumnType("varchar(450)")
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(c => c.DataApontamento)
                .HasColumnName("LANC_DATA_LANCAMENTO")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.QtdHoras)
                .HasColumnName("LANC_QTD_HORAS")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(c => c.PercentualConcluido)
                .HasColumnName("LANC_PERCENTUAL_CONCLUIDO")
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
                .HasColumnName("LANC_DATA_INCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("LANC_DATA_ALTERACAO")
                .HasColumnType("datetime");

            builder
                .HasOne(p => p.Tarefa)
                .WithMany(b => b.ListaApontamentos)
                .HasForeignKey(f => f.IdTarefa);
        }
    }
}
