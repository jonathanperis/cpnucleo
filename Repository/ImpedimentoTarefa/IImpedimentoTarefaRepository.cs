using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa
{
    public interface IImpedimentoTarefaRepository : IRepository<ImpedimentoTarefaItem>
    {
        Task<IList<ImpedimentoTarefaItem>> ListarPoridTarefa(int idTarefa);
    }
}