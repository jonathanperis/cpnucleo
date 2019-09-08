using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cpnucleo.Infra.Data.Mappings
{
    class RecursoProjetoMap : IEntityTypeConfiguration<RecursoProjeto>
    {
        public void Configure(EntityTypeBuilder<RecursoProjeto> builder)
        {
            builder.ToTable("CPN_TB_RECURSO_PROJETO");

            builder.Property(c => c.Id)
                .HasColumnName("RPROJ_ID");

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
                .HasColumnType("datetime");

            builder.Ignore(c => c.DataAlteracao);

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
