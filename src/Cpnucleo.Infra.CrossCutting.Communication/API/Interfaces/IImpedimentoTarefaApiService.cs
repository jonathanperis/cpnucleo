using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces
{
    public interface IImpedimentoTarefaApiService : ICrudApiService<ImpedimentoTarefaViewModel>
    {
        Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarPorTarefaAsync(string token, Guid idTarefa);
    }
}
