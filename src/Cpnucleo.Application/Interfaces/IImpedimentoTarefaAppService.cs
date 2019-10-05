using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IImpedimentoTarefaAppService : ICrudAppService<ImpedimentoTarefaViewModel>
    {
        IEnumerable<ImpedimentoTarefaViewModel> ListarPorTarefa(Guid idTarefa);
    }
}
