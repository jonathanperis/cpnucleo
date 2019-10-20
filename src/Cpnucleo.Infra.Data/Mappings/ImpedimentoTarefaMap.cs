using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Cpnucleo.Infra.Data.Mappings
{
    internal class ImpedimentoTarefaMap : IEntityTypeConfiguration<ImpedimentoTarefa>
    {
        public void Configure(EntityTypeBuilder<ImpedimentoTarefa> builder)
        {
            builder.ToTable("CPN_TB_TAREFA_IMPEDIMENTO");

            builder.Property(c => c.Id)
                .HasColumnName("ITAR_ID")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValue(Guid.NewGuid())
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
                .HasDefaultValue(DateTime.Now)
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
                .HasDefaultValue(true)
                .IsRequired();

            builder
                .HasOne(p => p.Tarefa)
                .WithMany(b => b.ListaImpedimentos)
                .HasForeignKey(f => f.IdTarefa)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(p => p.Impedimento)
                .WithMany()
                .HasForeignKey(f => f.IdImpedimento)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
