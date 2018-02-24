using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Projeto
{
    public class ProjetoContext : DbContext
    {
        public ProjetoContext(DbContextOptions<ProjetoContext> options)
            : base(options)
        { }

        public DbSet<ProjetoItem> Projetos { get; set; }   
    } 
}