using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IRecursoTarefaRepository : IRepository<RecursoTarefaItem>
    {
        Task<IEnumerable<RecursoTarefaItem>> ListarPoridTarefa(int idTarefa);

        Task<IEnumerable<RecursoTarefaItem>> ListarPoridRecurso(int idRecurso);
    }
}