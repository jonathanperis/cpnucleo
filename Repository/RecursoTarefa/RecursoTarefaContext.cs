using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.RecursoTarefa
{
    public class RecursoTarefaContext : DbContext
    {
        public RecursoTarefaContext(DbContextOptions<RecursoTarefaContext> options)
            : base(options)
        { }

        public DbSet<RecursoTarefaItem> RecursoTarefas { get; set; }   
    } 
}