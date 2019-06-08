using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository.RecursoTarefa
{
    public interface IRecursoTarefaRepository : IRepository<RecursoTarefaItem>
    {
        Task<IEnumerable<RecursoTarefaItem>> ListarPoridTarefa(int idTarefa);

        Task<IEnumerable<RecursoTarefaItem>> ListarPoridRecurso(int idRecurso);
    }
}