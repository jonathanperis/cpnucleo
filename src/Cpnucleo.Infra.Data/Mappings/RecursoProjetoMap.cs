using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Cpnucleo.Infra.Data.Mappings
{
    internal class RecursoProjetoMap : IEntityTypeConfiguration<RecursoProjeto>
    {
        public void Configure(EntityTypeBuilder<RecursoProjeto> builder)
        {
            builder.ToTable("CPN_TB_RECURSO_PROJETO");

            builder.Property(c => c.Id)
                .HasColumnName("RPROJ_ID")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.IdRecurso)
                .HasColumnName("REC_ID")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.IdProjeto)
                .HasColumnName("PROJ_ID")
                .HasColumnType("uniqueidentifier")
                .IsRequired();

            builder.Property(c => c.DataInclusao)
                .HasColumnName("RPROJ_DATA_INCLUSAO")
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Ignore(c => c.DataAlteracao);

            builder.Property(c => c.DataExclusao)
                .HasColumnName("RPROJ_DATA_EXCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.Ativo)
                .HasColumnName("RPROJ_ATIVO")
                .HasColumnType("bit")
                .IsRequired();

            builder
                .HasOne(p => p.Recurso)
                .WithMany()
                .HasForeignKey(f => f.IdRecurso);

            builder
                .HasOne(p => p.Projeto)
                .WithMany()
                .HasForeignKey(f => f.IdProjeto);
        }
    }
}
