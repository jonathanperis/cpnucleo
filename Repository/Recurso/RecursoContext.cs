using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Recurso
{
    public class RecursoContext : DbContext
    {
        public RecursoContext(DbContextOptions<RecursoContext> options)
            : base(options)
        { }

        public DbSet<RecursoItem> Recursos { get; set; }     
    } 
}