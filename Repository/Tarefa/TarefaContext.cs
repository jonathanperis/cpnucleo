using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Tarefa
{
    public class TarefaContext : DbContext
    {
        public TarefaContext(DbContextOptions<TarefaContext> options)
            : base(options)
        { }

        public DbSet<TarefaItem> Tarefas { get; set; }   
    } 
}