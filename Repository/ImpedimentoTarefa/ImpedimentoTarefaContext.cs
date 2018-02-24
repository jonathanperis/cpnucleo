using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa
{
    public class ImpedimentoTarefaContext : DbContext
    {
        public ImpedimentoTarefaContext(DbContextOptions<ImpedimentoTarefaContext> options)
            : base(options)
        { }

        public DbSet<ImpedimentoTarefaItem> ImpedimentoTarefas { get; set; }   
    } 
}