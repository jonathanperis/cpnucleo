using Cpnucleo.Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Interfaces
{
    public interface IImpedimentoTarefaAppService : IAppService<ImpedimentoTarefaViewModel>
    {
        IEnumerable<ImpedimentoTarefaViewModel> ListarPoridTarefa(Guid idTarefa);
    }
}
