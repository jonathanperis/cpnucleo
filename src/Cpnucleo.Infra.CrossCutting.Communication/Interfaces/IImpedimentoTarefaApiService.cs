using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Infra.CrossCutting.Communication.Interfaces
{
    public interface IImpedimentoTarefaApiService : ICrudApiService<ImpedimentoTarefaViewModel>
    {
        IEnumerable<ImpedimentoTarefaViewModel> ListarPorTarefa(string token, Guid idTarefa);
    }
}
