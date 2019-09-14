using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IRecursoTarefaRepository : IRepository<RecursoTarefaModel>
    {
        Task<IEnumerable<RecursoTarefaModel>> ListarPoridTarefaAsync(int idTarefa);

        Task<IEnumerable<RecursoTarefaModel>> ListarPoridRecursoAsync(int idRecurso);
    }
}