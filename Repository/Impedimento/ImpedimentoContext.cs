using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Impedimento
{
    public class ImpedimentoContext : DbContext
    {
        public ImpedimentoContext(DbContextOptions<ImpedimentoContext> options)
            : base(options)
        { }

        public DbSet<ImpedimentoItem> Impedimentos { get; set; }
    } 
}