using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.RecursoProjeto
{
    public class RecursoProjetoContext : DbContext
    {
        public RecursoProjetoContext(DbContextOptions<RecursoProjetoContext> options)
            : base(options)
        { }

        public DbSet<RecursoProjetoItem> RecursoProjetos { get; set; }   
    } 
}