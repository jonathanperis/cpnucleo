using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository.RecursoProjeto
{
    public interface IRecursoProjetoRepository : IRepository<RecursoProjetoItem>
    {
        Task<IEnumerable<RecursoProjetoItem>> ListarPoridProjeto(int idProjeto);
    }
}