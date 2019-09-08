using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cpnucleo.Infra.Data.Mappings
{
    class ProjetoMap : IEntityTypeConfiguration<Projeto>
    {
        public void Configure(EntityTypeBuilder<Projeto> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("PROJ_ID");

            builder.Property(c => c.Nome)
                .HasColumnName("PROJ_NOME")
                .HasColumnType("varchar(80)")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(c => c.IdSistema)
                .HasColumnName("SIS_ID")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(c => c.DataInclusao)
                .HasColumnName("PROJ_DATA_INCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("PROJ_DATA_ALTERACAO")
                .HasColumnType("datetime");
        }
    }
}
