using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IApontamentoRepository : IRepository<ApontamentoItem>
    {
        Task ApontarHorasAsync(ApontamentoItem apontamento);

        Task<int> ObterTotalHorasPoridRecursoAsync(int idRecurso, int idTarefa);

        Task<IEnumerable<ApontamentoItem>> ListarPoridRecursoAsync(int idRecurso);
    }
}