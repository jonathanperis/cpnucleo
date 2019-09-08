using Cpnucleo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cpnucleo.Infra.Data.Mappings
{
    class RecursoMap : IEntityTypeConfiguration<Recurso>
    {
        public void Configure(EntityTypeBuilder<Recurso> builder)
        {
            builder.ToTable("CPN_TB_RECURSO");

            builder.Property(c => c.Id)
                .HasColumnName("REC_ID");

            builder.Property(c => c.Nome)
                .HasColumnName("REC_NOME")
                .HasColumnType("varchar(80)")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(c => c.Ativo)
                .HasColumnName("REC_ATIVO")
                .HasColumnType("bit");

            builder.Property(c => c.Login)
                .HasColumnName("REC_LOGIN")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Senha)
                .HasColumnName("REC_SENHA")
                .HasColumnType("varchar(64)")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(c => c.Salt)
                .HasColumnName("REC_SENHA_SALT")
                .HasColumnType("varchar(64)")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(c => c.DataInclusao)
                .HasColumnName("REC_DATA_INCLUSAO")
                .HasColumnType("datetime");

            builder.Property(c => c.DataAlteracao)
                .HasColumnName("REC_DATA_ALTERACAO")
                .HasColumnType("datetime");
        }
    }
}
