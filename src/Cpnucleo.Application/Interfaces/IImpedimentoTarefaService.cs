using Cpnucleo.Application.ViewModels;
using System;
using System.Linq;

namespace Cpnucleo.Application.Interfaces
{
    public interface IImpedimentoTarefaAppService : IAppService<ImpedimentoTarefaViewModel>
    {
        IQueryable<ImpedimentoTarefaViewModel> ListarPoridTarefa(Guid idTarefa);
    }
}
