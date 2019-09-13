using Cpnucleo.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IApontamentoAppService : IAppService<ApontamentoViewModel>
    {
        new bool Incluir(ApontamentoViewModel apontamento);

        int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<ApontamentoViewModel> ListarPoridRecurso(Guid idRecurso);
    }
}
