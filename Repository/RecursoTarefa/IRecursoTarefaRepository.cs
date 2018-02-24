using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository.RecursoTarefa
{
    public interface IRecursoTarefaRepository : IRepository<RecursoTarefaItem>
    {
        Task<IList<RecursoTarefaItem>> ListarPoridTarefa(int idTarefa);

        Task<IList<RecursoTarefaItem>> ListarPoridRecurso(int idRecurso);
    }
}