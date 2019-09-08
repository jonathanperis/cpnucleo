using Cpnucleo.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IApontamentoAppService : IAppService<ApontamentoViewModel>
    {
        void ApontarHoras(ApontamentoViewModel apontamento);

        int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<ApontamentoViewModel> ListarPoridRecurso(Guid idRecurso);
    }
}
