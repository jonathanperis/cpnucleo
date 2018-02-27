using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository.RecursoTarefa
{
    public interface IRecursoTarefaRepository : IRepository<RecursoTarefaItem>
    {
        Task<IEnumerable<RecursoTarefaItem>> ListarPoridTarefa(int idTarefa);

        Task<IEnumerable<RecursoTarefaItem>> ListarPoridRecurso(int idRecurso);
    }
}