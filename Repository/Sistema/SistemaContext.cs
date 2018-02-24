using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Sistema
{
    public class SistemaContext : DbContext
    {
        public SistemaContext(DbContextOptions<SistemaContext> options)
            : base(options)
        { }

        public DbSet<SistemaItem> Sistemas { get; set; }
    } 
}