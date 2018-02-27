using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa
{
    public interface IImpedimentoTarefaRepository : IRepository<ImpedimentoTarefaItem>
    {
        Task<IEnumerable<ImpedimentoTarefaItem>> ListarPoridTarefa(int idTarefa);
    }
}