using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IApontamentoAppService : ICrudAppService<ApontamentoViewModel>
    {
        int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa);

        IEnumerable<ApontamentoViewModel> ListarPorRecurso(Guid idRecurso);
    }
}
