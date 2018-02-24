using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.TipoTarefa
{
    public class TipoTarefaContext : DbContext
    {
        public TipoTarefaContext(DbContextOptions<TipoTarefaContext> options)
            : base(options)
        { }

        public DbSet<TipoTarefaItem> TipoTarefas { get; set; }
    } 
}