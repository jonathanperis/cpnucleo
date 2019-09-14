using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IImpedimentoTarefaRepository : IRepository<ImpedimentoTarefaModel>
    {
        Task<IEnumerable<ImpedimentoTarefaModel>> ListarPoridTarefaAsync(int idTarefa);
    }
}