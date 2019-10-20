using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Cpnucleo.Infra.Data.Mappings
{
    internal class WorkflowMap : IEntityTypeConfiguration<Workflow>
    {
        public void Configure(EntityTypeBuilder<Workflow> builder)
        {
            builder.ToTable("CPN_TB_WORKFLOW");

            builder.Property(c => c.Id)
                .HasColumnName("WOR_ID")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValue(Guid.NewGuid())
                .IsRequired();

            builder.Property(c => c.Nome)
                .HasColumnName("WOR_NOME")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Ordem)
                .HasColumnName("WOR_ORDEM")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(c => c.DataInclusao)
                .HasColumnName("WOR_DATA_INCLUSAO")
                .HasColumnType("datetime")
                .HasDefaultValue(DateTime.Now)
                .IsRequired();

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("WOR_DATA_ALTERACAO")
                .HasColumnType("datetime");

            builder.Property(c => c.DataExclusao)
                .HasColumnName("WOR_DATA_EXCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.Ativo)
                .HasColumnName("WOR_ATIVO")
                .HasColumnType("bit")
                .HasDefaultValue(true)
                .IsRequired();

            builder
                .HasMany(c => c.ListaTarefas)
                .WithOne(c => c.Workflow)
                .HasForeignKey(f => f.IdWorkflow)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
