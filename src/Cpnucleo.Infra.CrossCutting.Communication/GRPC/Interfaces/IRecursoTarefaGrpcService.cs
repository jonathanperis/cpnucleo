using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces
{
    public interface IRecursoTarefaGrpcService : ICrudGrpcService<RecursoTarefaViewModel>
    {
        Task<IEnumerable<RecursoTarefaViewModel>> ListarPorTarefaAsync(Guid idTarefa);
    }
}
