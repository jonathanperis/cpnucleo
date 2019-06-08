using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository.Apontamento
{
    public interface IApontamentoRepository : IRepository<ApontamentoItem>
    {
        Task<int> ObterTotalHorasPoridRecurso(int idRecurso, int idTarefa);

        Task<IEnumerable<ApontamentoItem>> ListarPoridRecurso(int idRecurso);
    }
}