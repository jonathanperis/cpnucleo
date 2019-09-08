using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cpnucleo.Infra.Data.Mappings
{
    class RecursoTarefaMap : IEntityTypeConfiguration<RecursoTarefa>
    {
        public void Configure(EntityTypeBuilder<RecursoTarefa> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("RTAR_ID");

            builder.Property(c => c.PercentualTarefa)
                .HasColumnName("RTAR_PERCENTUAL")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(c => c.IdRecurso)
                .HasColumnName("REC_ID")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(c => c.IdTarefa)
                .HasColumnName("TAR_ID")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(c => c.DataInclusao)
                .HasColumnName("RTAR_DATA_INCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("RTAR_DATA_ALTERACAO")
                .HasColumnType("datetime");
        }
    }
}
