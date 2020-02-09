using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Cpnucleo.Infra.Data.Mappings
{
    internal class ProjetoMap : IEntityTypeConfiguration<Projeto>
    {
        public void Configure(EntityTypeBuilder<Projeto> builder)
        {
            builder.ToTable("CPN_TB_PROJETO");

            builder.Property(c => c.Id)
                .HasColumnName("PROJ_ID")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.Nome)
                .HasColumnName("PROJ_NOME")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.IdSistema)
                .HasColumnName("SIS_ID")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.DataInclusao)
                .HasColumnName("PROJ_DATA_INCLUSAO")
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("PROJ_DATA_ALTERACAO")
                .HasColumnType("datetime");

            builder.Property(c => c.DataExclusao)
                .HasColumnName("PROJ_DATA_EXCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.Ativo)
                .HasColumnName("PROJ_ATIVO")
                .HasColumnType("bit")
                .IsRequired();

            builder
                .HasOne(p => p.Sistema)
                .WithMany()
                .HasForeignKey(f => f.IdSistema);
        }
    }
}
