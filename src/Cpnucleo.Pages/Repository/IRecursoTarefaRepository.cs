using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IRecursoTarefaRepository : IRepository<RecursoTarefaItem>
    {
        Task<IEnumerable<RecursoTarefaItem>> ListarPoridTarefaAsync(int idTarefa);

        Task<IEnumerable<RecursoTarefaItem>> ListarPoridRecursoAsync(int idRecurso);
    }
}