using Cpnucleo.Pages.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public interface IApontamentoRepository : IRepository<ApontamentoModel>
    {
        Task ApontarHorasAsync(ApontamentoModel apontamento);

        Task<int> ObterTotalHorasPoridRecursoAsync(int idRecurso, int idTarefa);

        Task<IEnumerable<ApontamentoModel>> ListarPoridRecursoAsync(int idRecurso);
    }
}