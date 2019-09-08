using Cpnucleo.Application.ViewModels;
using System;
using System.Linq;

namespace Cpnucleo.Application.Interfaces
{
    public interface IApontamentoAppService : IAppService<ApontamentoViewModel>
    {
        void ApontarHoras(ApontamentoViewModel apontamento);

        int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa);

        IQueryable<ApontamentoViewModel> ListarPoridRecurso(Guid idRecurso);
    }
}
