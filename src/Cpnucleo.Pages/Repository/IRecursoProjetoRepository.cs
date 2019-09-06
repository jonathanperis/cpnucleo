using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IRecursoProjetoRepository : IRepository<RecursoProjetoModel>
    {
        Task<IEnumerable<RecursoProjetoModel>> ListarPoridProjetoAsync(int idProjeto);
    }
}