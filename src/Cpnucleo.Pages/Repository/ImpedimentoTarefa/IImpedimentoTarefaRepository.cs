using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository.ImpedimentoTarefa
{
    public interface IImpedimentoTarefaRepository : IRepository<ImpedimentoTarefaItem>
    {
        Task<IEnumerable<ImpedimentoTarefaItem>> ListarPoridTarefa(int idTarefa);
    }
}