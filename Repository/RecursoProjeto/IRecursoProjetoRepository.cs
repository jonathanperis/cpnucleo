using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository.RecursoProjeto
{
    public interface IRecursoProjetoRepository : IRepository<RecursoProjetoItem>
    {
        Task<IList<RecursoProjetoItem>> ListarPoridProjeto(int idProjeto);
    }
}