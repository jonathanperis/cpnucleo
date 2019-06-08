using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IApontamentoRepository : IRepository<ApontamentoItem>
    {
        Task<int> ObterTotalHorasPoridRecurso(int idRecurso, int idTarefa);

        Task<IEnumerable<ApontamentoItem>> ListarPoridRecurso(int idRecurso);
    }
}