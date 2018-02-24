using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Apontamento
{
    public class ApontamentoContext : DbContext
    {
        public ApontamentoContext(DbContextOptions<ApontamentoContext> options)
            : base(options)
        { }

        public DbSet<ApontamentoItem> Apontamentos { get; set; }   
    } 
}