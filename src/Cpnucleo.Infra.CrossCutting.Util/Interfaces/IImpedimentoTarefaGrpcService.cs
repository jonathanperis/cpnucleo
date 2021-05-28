using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    public interface IImpedimentoTarefaGrpcService : IGenericGrpcService<ImpedimentoTarefaViewModel>
    {
        Task<IEnumerable<ImpedimentoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa);
    }
}
