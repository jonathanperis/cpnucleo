using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cpnucleo.Infra.Data.Mappings
{
    internal class ImpedimentoMap : IEntityTypeConfiguration<Impedimento>
    {
        public void Configure(EntityTypeBuilder<Impedimento> builder)
        {
            builder.ToTable("CPN_TB_IMPEDIMENTO");

            builder.Property(c => c.Id)
                .HasColumnName("IMP_ID");

            builder.Property(c => c.Nome)
                .HasColumnName("IMP_NOME")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.DataInclusao)
                .HasColumnName("IMP_DATA_INCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("IMP_DATA_ALTERACAO")
                .HasColumnType("datetime");
        }
    }
}
