using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IRecursoProjetoRepository : IRepository<RecursoProjetoItem>
    {
        Task<IEnumerable<RecursoProjetoItem>> ListarPoridProjeto(int idProjeto);
    }
}