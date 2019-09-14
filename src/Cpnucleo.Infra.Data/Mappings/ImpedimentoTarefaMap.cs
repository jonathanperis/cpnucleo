using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cpnucleo.Infra.Data.Mappings
{
    internal class ImpedimentoTarefaMap : IEntityTypeConfiguration<ImpedimentoTarefa>
    {
        public void Configure(EntityTypeBuilder<ImpedimentoTarefa> builder)
        {
            builder.ToTable("CPN_TB_TAREFA_IMPEDIMENTO");

            builder.Property(c => c.Id)
                .HasColumnName("ITAR_ID");

            builder.Property(c => c.Descricao)
                .HasColumnName("ITAR_DESCRICAO")
                .HasColumnType("varchar(450)")
                .HasMaxLength(450)
                .IsRequired();

            builder.Property(c => c.Ativo)
                .HasColumnName("ITAR_ATIVO")
                .HasColumnType("bit");

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
                .HasColumnType("datetime");

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("ITAR_DATA_ALTERACAO")
                .HasColumnType("datetime");

            builder
                .HasOne(p => p.Tarefa)
                .WithMany(b => b.ListaImpedimentos)
                .HasForeignKey(f => f.IdTarefa);

            builder
                .HasOne(p => p.Impedimento)
                .WithMany()
                .HasForeignKey(f => f.IdImpedimento);
        }
    }
}
