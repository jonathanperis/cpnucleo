using Cpnucleo.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IApontamentoAppService : ICrudAppService<ApontamentoViewModel>
    {
        new bool Incluir(ApontamentoViewModel apontamento);

        int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<ApontamentoViewModel> ListarPorRecurso(Guid idRecurso);
    }
}
