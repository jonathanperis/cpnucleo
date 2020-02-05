using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces
{
    public interface IImpedimentoTarefaGrpcService : ICrudGrpcService<ImpedimentoTarefaViewModel>
    {
        Task<IEnumerable<ImpedimentoTarefaViewModel>> ListarPorTarefaAsync(Guid idTarefa);
    }
}
